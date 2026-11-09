using System;
using System.Linq;
using Mario.Game.Assets;
using Mario.Game.Audio;
using Mario.Game.Controllers;
using Mario.Game.Entities.Enemies;
using Mario.Game.Entities.Items;
using Mario.Game.Entities.Players;
using Mario.Game.Entities.Projectiles;
using Mario.Game.Interfaces;
using Mario.Game.Interfaces.Player;
using Mario.Game.Levels;
using Mario.Game.Physics;
using Mario.Game.Rendering;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Mario.Game.State;

public sealed class GameWorld : IEntitySpawner, IDisposable
{
    private const int MaxFireballs = 4;
    private const int PowerUpScore = 1000;
    private const int OverworldRoom = 0;
    private const int UnderwaterRoom = 1;
    private const float PipeExitOffset = 144f;
    private static readonly string[] RoomFiles = { "1-1.csv", "underwater.csv" };

    private readonly MarioGame game;
    private readonly ContentManager content;
    private readonly SpriteFactory spriteFactory;
    private readonly ProjectileFactory projectileFactory;
    private readonly GameEntities gameEntities = new();
    private readonly PhysicsEngine physicsEngine = new(gravity: 1000f);
    private readonly GameStats stats;
    private readonly WorldRenderer renderer;
    private readonly SoundManager sounds;
    private IController keyboardController = null!;
    private IController mouseController = null!;
    private int currentRoom;
    private bool reloadRoomRequested;
    private bool warpRequested;
    private Vector2 returnPosition;

    public bool IsGameOver => stats.Lives <= 0;
    public bool IsCourseClear { get; private set; }

    public GameWorld(MarioGame game, AssetManager assets, GameStats stats, WorldRenderer renderer)
    {
        this.game = game;
        content = game.Content;
        this.stats = stats;
        this.renderer = renderer;
        spriteFactory = new SpriteFactory(assets);
        projectileFactory = new ProjectileFactory(spriteFactory, this);
        sounds = new SoundManager(assets);
        ResetGame();
    }

    public bool TrySpawnProjectile(ProjectileType type, Vector2 position, Vector2 velocity)
    {
        // Check limits before constructing the projectile or allocating its sprite.
        if (type == ProjectileType.Fireball &&
            gameEntities.GetActiveProjectileCount<Fireball>() >= MaxFireballs)
            return false;

        gameEntities.QueueSpawn(projectileFactory.Create(type, position, velocity));
        return true;
    }

    public void Spawn(IGameObject entity)
    {
        if (entity is IItem item)
            item.Collected += OnItemCollected;
        if (entity is IBlock block)
            block.SoundRequested += sounds.Play;
        gameEntities.QueueSpawn(entity);
    }

    public void ResetGame()
    {
        stats.Reset();
        LoadRoom(0);
    }

    public void NextRoom()
    {
        LoadRoom((currentRoom + 1) % RoomFiles.Length);
    }

    public void PreviousRoom()
    {
        LoadRoom((currentRoom - 1 + RoomFiles.Length) % RoomFiles.Length);
    }

    public void ToggleHitboxes() => renderer.ToggleHitboxes();

    private void LoadRoom(int roomIndex, MarioPower power = MarioPower.Small, Vector2? spawn = null)
    {
        IsCourseClear = false;
        currentRoom = roomIndex;
        gameEntities.Reset();
        reloadRoomRequested = false;
        warpRequested = false;
        sounds.PlayMusic();

        var level = LevelLoader.LoadLevel(content, RoomFiles[currentRoom], gameEntities, spriteFactory, this);

        Vector2 marioSpawn = spawn ?? level.spawn;
        renderer.ConfigureRoom(level.width, level.height, roomIndex == UnderwaterRoom);

        foreach (IBlock block in gameEntities.Blocks)
        {
            block.SoundRequested += sounds.Play;
        }

        foreach (IItem item in gameEntities.Items)
        {
            item.Collected += OnItemCollected;
        }

        IPlayer mario = CreateMario(roomIndex == UnderwaterRoom, marioSpawn);
        mario.SetPower(power);
        gameEntities.Players.Add(mario);
        foreach (Lakitu lakitu in gameEntities.Enemies.OfType<Lakitu>())
            lakitu.Follow(mario);
        foreach (BillLauncher billLauncher in gameEntities.Enemies.OfType<BillLauncher>())
            billLauncher.Follow(mario);
    }

    // Keys 1-6: switch between regular and swimming Mario, then set his size
    public void ChangeMario(bool swimming, MarioPower power)
    {
        foreach (IPlayer player in gameEntities.Players)
        {
            player.SetSwimming(swimming);
            player.SetPower(power);
        }
    }

    private IPlayer CreateMario(bool swimming, Vector2 position)
    {
        IPlayer player = new Player(spriteFactory, this, position);
        player.WarpRequested += OnWarpRequested;
        player.SetSwimming(swimming);
        player.SoundRequested += sounds.Play;
        player.DeathStarted += OnMarioDeathStarted;
        player.Died += OnMarioDied;
        player.FlagpoleReached += OnFlagpoleReached;
        keyboardController = new KeyboardController(game, this, player);
        mouseController = new MouseController(this);
        return player;
    }

    private void OnWarpRequested()
    {
        warpRequested = true;
    }

    private void Warp()
    {
        MarioPower power = gameEntities.Players[0].MarioPowerState.Power;

        if (currentRoom == UnderwaterRoom)
        {
            LoadRoom(OverworldRoom, power, returnPosition);
        }
        else
        {
            // Land past the pipe so holding Down doesn't warp Mario straight back
            returnPosition = gameEntities.Players[0].Position + new Vector2(PipeExitOffset, 0f);
            LoadRoom(UnderwaterRoom, power);
        }
    }

    private void OnMarioDeathStarted()
    {
        sounds.StopMusic();
        sounds.Play(SoundType.Death);
    }

    // Mario can die in the middle of a collision loop, so the room is reloaded at the end of Update instead
    private void OnMarioDied()
    {
        stats.LoseLife();
        reloadRoomRequested = true;
    }

    private void OnFlagpoleReached()
    {
        IsCourseClear = true;
        sounds.StopMusic();
    }

    private void OnItemCollected(ItemType type)
    {
        if (type == ItemType.Coin)
        {
            stats.AddCoin();
            sounds.Play(SoundType.Coin);
        }
        else
        {
            stats.AddScore(PowerUpScore);
            sounds.Play(SoundType.PowerUp);
        }
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
            {
                player.SetPower(MarioPower.Dead);
            }
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
            {
                return;
            }

            LoadRoom(currentRoom);
        }

        if (warpRequested)
        {
            warpRequested = false;
            Warp();
        }
    }

    public void Draw() => renderer.Draw(gameEntities);

    public void Dispose() => renderer.Dispose();
}
