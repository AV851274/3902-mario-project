using System;
using System.Linq;
using Mario.Game.Assets;
using Mario.Game.Audio;
using Mario.Game.Controllers;
using Mario.Game.Entities.Blocks;
using Mario.Game.Entities.Items;
using Mario.Game.Entities.Players;
using Mario.Game.Entities.Projectiles;
using Mario.Game.Interfaces;
using Mario.Game.Interfaces.Player;
using Mario.Game.Physics;
using Mario.Game.Rendering;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;

namespace Mario.Game.State;

public sealed class GameWorld : IEntitySpawner, IGameControls, IDisposable
{
    private const int MaxFireballs = 4;

    private readonly SpriteFactory spriteFactory;
    private readonly ProjectileFactory projectileFactory;
    private readonly ItemFactory itemFactory;
    private readonly BlockFactory blockFactory;
    private readonly GameEntities gameEntities = new();
    private readonly RoomManager roomManager;
    private readonly MarioGame game;
    private IController keyboardController = null!;
    private IController mouseController = null!;
    private readonly IGameAudio audio;
    private readonly PhysicsEngine physicsEngine = new(gravity: 1000f);
    private readonly GameStats stats;
    private readonly WorldRenderer renderer;

    private bool reloadRoomRequested;
    private bool warpRequested;

    public bool IsGameOver => stats.Lives <= 0;
    public bool IsCourseClear { get; private set; }

    public GameWorld(
        MarioGame game,
        AssetManager assets,
        GameStats stats,
        WorldRenderer renderer)
    {
        this.stats = stats;
        this.renderer = renderer;

        spriteFactory = new SpriteFactory(assets);
        roomManager = new RoomManager(game.Content);
        projectileFactory = new ProjectileFactory(spriteFactory, this);
        audio = new GameAudio(assets);
        itemFactory = new ItemFactory(spriteFactory, stats, audio);
        blockFactory = new BlockFactory(spriteFactory, itemFactory, this, audio);
        this.game = game;

        ResetGame();
    }

    public bool TrySpawnProjectile(
        ProjectileType type,
        Vector2 position,
        Vector2 velocity)
    {
        if (type == ProjectileType.Fireball &&
            gameEntities.GetActiveProjectileCount<Fireball>() >= MaxFireballs)
            return false;

        gameEntities.QueueSpawn(
            projectileFactory.Create(type, position, velocity)
        );

        return true;
    }

    public void Spawn(IGameObject entity) =>
        gameEntities.QueueSpawn(entity);

    public void ResetGame()
    {
        stats.Reset();
        InitializeRoom(0);
    }

    public void NextRoom() =>
        InitializeRoom(roomManager.NextRoomIndex);

    public void PreviousRoom() =>
        InitializeRoom(roomManager.PreviousRoomIndex);

    public void ToggleHitboxes() =>
        renderer.ToggleHitboxes();

    private void InitializeRoom(int roomIndex, MarioPower power = MarioPower.Small, Vector2? spawn = null)
    {
        IsCourseClear = false;
        reloadRoomRequested = false;
        warpRequested = false;

        audio.StartRoomMusic();

        var level = roomManager.LoadRoom(roomIndex, gameEntities, spriteFactory, this, itemFactory, blockFactory);

        Vector2 marioSpawn = spawn ?? level.spawn;

        renderer.ConfigureRoom(
            level.width,
            level.height,
            roomManager.IsUnderwater
        );

        IPlayer mario = new Player(spriteFactory, this, audio, marioSpawn);
        mario.SetSwimming(roomManager.IsUnderwater);

        keyboardController = new KeyboardController(game, this, mario, audio);
        mouseController = new MouseController(this);

        mario.WarpRequested += OnWarpRequested;
        mario.Died += OnMarioDied;
        mario.FlagpoleReached += OnFlagpoleReached;

        mario.SetPower(power);
        gameEntities.Players.Add(mario);

        foreach (IPlayerFollower enemy in gameEntities.Enemies.OfType<IPlayerFollower>())
        {
            enemy.Follow(mario);
        }
    }

    private void OnWarpRequested()
    {
        warpRequested = true;
    }

    private void Warp()
    {
        MarioPower power =
            gameEntities.Players[0].MarioPowerState.Power;

        var destination = roomManager.GetWarpDestination(
            gameEntities.Players[0].Position
        );

        InitializeRoom(
            destination.roomIndex,
            power,
            destination.spawn
        );
    }

    private void OnMarioDied()
    {
        stats.LoseLife();
        reloadRoomRequested = true;
    }

    private void OnFlagpoleReached()
    {
        IsCourseClear = true;
    }

    public void Update(GameTime gameTime)
    {
        if (IsCourseClear)
            return;

        keyboardController.Update(gameTime);
        mouseController.Update(gameTime);

        gameEntities.Update(gameTime);
        physicsEngine.Step(gameEntities, gameTime);

        if (IsCourseClear)
            return;

        stats.Update(gameTime);

        if (stats.IsOutOfTime)
        {
            foreach (IPlayer player in gameEntities.Players)
                player.SetPower(MarioPower.Dead);
        }

        gameEntities.CommitSpawns();

        foreach (IPlayer player in gameEntities.Players)
        {
            player.UpdateAnimation(gameTime);
        }

        renderer.Follow(gameEntities.Players[0].Position);

        if (reloadRoomRequested)
        {
            reloadRoomRequested = false;

            if (stats.Lives <= 0)
                return;

            InitializeRoom(roomManager.CurrentRoom);
        }

        if (warpRequested)
        {
            warpRequested = false;
            Warp();
        }
    }

    public void Draw() =>
        renderer.Draw(gameEntities);

    public void Dispose() =>
        renderer.Dispose();
}
