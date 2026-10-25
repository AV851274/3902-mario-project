using System.Linq;
using Mario.Game.Interfaces.Player;
using Mario.Game.Animation;
using Mario.Game.Controllers;
using Mario.Game.Entities.Enemies;
using Mario.Game.Entities.Items;
using Mario.Game.Interfaces;
using Mario.Game.Entities.Players;
using Mario.Game.Entities.Projectiles;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mario.Game.State;
using Mario.Game.Levels;
using Mario.Game.Rendering.Camera;
using Mario.Game.Physics;

namespace Mario.Game;

public class MarioGame : Microsoft.Xna.Framework.Game
{
    private const int MaxFireBall = 4;
    
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private IController keyboardController;
    private IController mouseController;
    private SpriteFactory spriteFactory;
    private GameEntities gameEntities = new GameEntities();
    private Camera2D camera = new Camera2D();
    private int levelWidth;
    private readonly PhysicsEngine physicsEngine;

    // Rooms in order; the mouse cycles through them (left = previous, right = next)
    private static readonly string[] RoomFiles = { "testRoom.csv", "level1.csv", "room2.csv", "1-1.csv" };
    private int currentRoom;
    private bool reloadRoomRequested;

    public MarioGame()
    {
        physicsEngine = new PhysicsEngine(gravity: 1000f);
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 768;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        spriteFactory = new SpriteFactory();
        spriteFactory.LoadTextures(Content);

        ResetGame();
    }


    public void ResetGame()
    {
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

    private void LoadRoom(int roomIndex)
    {
        currentRoom = roomIndex;
        gameEntities.Reset();

        var level = LevelLoader.LoadLevel(Content, RoomFiles[currentRoom], gameEntities, spriteFactory);

        Vector2 marioSpawn = level.spawn;
        levelWidth = level.width;

        // Zoom so the whole level height fits in the window
        camera = new Camera2D(GraphicsDevice.Viewport.Height / (float)level.height);

        // Hammer Bros announce their throws; spawn the hammer projectile when they do
        foreach (IEnemy enemy in gameEntities.Enemies)
        {
            if (enemy is HammerBro hammerBro)
            {
                hammerBro.SummonHammer += SpawnHammer;
            }
            if (enemy is Bowser bowser)
            {
                bowser.SummonHammer += SpawnHammer;
            }
        }

        foreach (IBlock block in gameEntities.Blocks)
        {
            block.ItemReleased += SpawnItem;
        }

        gameEntities.Players.Add(CreateMario(false, marioSpawn));
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
        IPlayer player = new Player(spriteFactory.Mario, position);
        player.SetSwimming(swimming);
        player.SummonFireball += SpawnFireball;
        player.Died += OnMarioDied;
        keyboardController = new KeyboardController(this, player);
        mouseController = new MouseController(this);
        return player;
    }

    // Mario can die in the middle of a collision loop, so the room is reloaded at the end of Update instead
    private void OnMarioDied()
    {
        reloadRoomRequested = true;
    }

    private void SpawnFireball(Vector2 position, Direction direction)
    {
        if (gameEntities.Projectiles.OfType<Fireball>().Count(fireball => fireball.Active) >= MaxFireBall)
            return;

        float flip = direction == Direction.Left ? -1f : 1f;
        var velocity = new Vector2(500f * flip, 200f);
        gameEntities.Projectiles.Add(new Fireball(
            spriteFactory.Mario.CreateFireballSprite(), // new animation contoller for each fireball
            position + new Vector2(25 * flip, -20f),
            velocity,
            2f,
            400f
        ));
    }

    private void SpawnItem(ItemType type, Vector2 position)
    {
        Item item = new Item(spriteFactory.Items, type, position);
        item.StartEmerging();
        gameEntities.Items.Add(item);
    }

    private void SpawnHammer(Vector2 position, Direction direction)
    {
        float directionSign = direction == Direction.Left ? -1f : 1f;

        gameEntities.Projectiles.Add(new Hammer(
            spriteFactory.Enemies.CreateHammerSprite(),
            position + new Vector2(20f * directionSign, -20f),
            new Vector2(250f * directionSign, -350f),
            3f));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        keyboardController.Update(gameTime);
        mouseController.Update(gameTime);

        gameEntities.Update(gameTime);
        physicsEngine.Step(gameEntities, gameTime);

        foreach (IPlayer player in gameEntities.Players)
        {
            player.UpdateAnimation(gameTime);
        }
        camera.Follow(gameEntities.Players[0].Position, GraphicsDevice.Viewport.Width, levelWidth);

        if (reloadRoomRequested)
        {
            reloadRoomRequested = false;
            LoadRoom(currentRoom);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        spriteBatch.Begin(
            SpriteSortMode.Deferred,
            null,
            SamplerState.PointClamp,
            null,
            null,
            null,
            camera.Transform);

        foreach (var gameObj in gameEntities.GetAllGameObjects())
        {
            gameObj.Draw(spriteBatch);
        }

        spriteBatch.End();

        base.Draw(gameTime);
    }

}
