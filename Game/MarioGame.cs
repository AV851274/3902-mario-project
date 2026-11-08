using Mario.Game.Assets;
using System;
using System.Linq;
using Mario.Game.Interfaces.Player;
using Mario.Game.Animation;
using Mario.Game.Audio;
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
using Mario.Game.Rendering;
using Mario.Game.Rendering.Camera;
using Mario.Game.Physics;

namespace Mario.Game;

public class MarioGame : Microsoft.Xna.Framework.Game
{
    private const int MaxFireBall = 4;
    private const int PowerUpScore = 1000;
    private bool showHitboxes = false;
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private IController keyboardController;
    private IController mouseController;
    private SpriteFactory spriteFactory;
    private HitboxRenderer hitboxes;
    private GameEntities gameEntities = new GameEntities();
    private GameEntities entityBuffer = new GameEntities();
    private Camera2D camera = new Camera2D();
    private int levelWidth;
    private int levelHeight;
    private Texture2D pixel;
    private const int WaterTopY = 48;
    private static readonly Color WaterColor = new Color(0x42, 0x42, 0xFF);
    private readonly PhysicsEngine physicsEngine;
    private readonly GameStats stats = new GameStats();
    private Hud hud;
    private SoundManager sounds;

    // Rooms in order; the mouse cycles through them (left = previous, right = next)
    private static readonly string[] RoomFiles = { "1-1.csv", "underwater.csv" };
    private int currentRoom;
    private bool reloadRoomRequested;
    private const int OverworldRoom = 0;
    private const int UnderwaterRoom = 1;
    private const float PipeExitOffset = 144f;
    private Vector2 returnPosition;
    private bool warpRequested;
    private GameScreen currentScreen = GameScreen.Start;
    private KeyboardState previousKeyboardState;
    private SpriteFont screenFont;
    private StartScreenState startScreenState;
    private PausedScreenState pausedScreenState;
    private GameOverScreenState gameOverScreenState;

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
        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });

        var assets = new AssetManager(Content, "definitions/assets.json");
        spriteFactory = new SpriteFactory(assets);
        hitboxes = new HitboxRenderer(assets);
        screenFont = assets.GetFont("hud");
        hud = new Hud(screenFont, stats);
        startScreenState = new StartScreenState(
            spriteBatch,
            screenFont,
            GraphicsDevice
        );
        pausedScreenState = new PausedScreenState(
            spriteBatch,
            screenFont,
            GraphicsDevice
        );
        gameOverScreenState = new GameOverScreenState(
            spriteBatch,
            screenFont,
            GraphicsDevice
        );
        sounds = new SoundManager(assets);

        ResetGame();
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

    public void ToggleHitboxes()
    {
        showHitboxes = !showHitboxes;
    }

    private void LoadRoom(int roomIndex, MarioPower power = MarioPower.Small, Vector2? spawn = null)
    {
        currentRoom = roomIndex;
        gameEntities.Reset();
        entityBuffer.Reset();
        sounds.PlayMusic();

        var level = LevelLoader.LoadLevel(Content, RoomFiles[currentRoom], gameEntities, spriteFactory);

        Vector2 marioSpawn = spawn ?? level.spawn;
        levelWidth = level.width;
        levelHeight = level.height;

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
                bowser.SummonFire += SpawnFire;
            }
            if (enemy is ParaKoopa paraKoopa)
            {
                paraKoopa.Stomped += ReplaceParaKoopaWithTurtle;
            }
            if (enemy is Lakitu lakitu) {
                lakitu.SummonEgg += SpawnEgg;
            }
            if (enemy is BillLauncher billLauncher)
            {
                billLauncher.SummonBill += SpawnBill;
            }
        }
        foreach (IBlock block in gameEntities.Blocks)
        {
            block.ItemReleased += SpawnItem;
            block.SoundRequested += sounds.Play;
        }
        foreach (IItem item in gameEntities.Items)
        {
            item.Collected += OnItemCollected;
        }

        foreach (IProjectile projectile in gameEntities.Projectiles)
        {
            if (projectile is Egg egg)
            {
                egg.SummonSpiny += ReplaceEggWithSpiny;
            }
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
        IPlayer player = new Player(spriteFactory, position);
        player.WarpRequested += OnWarpRequested;
        player.SetSwimming(swimming);
        player.SummonFireball += SpawnFireball;
        player.SoundRequested += sounds.Play;
        player.DeathStarted += OnMarioDeathStarted;
        player.Died += OnMarioDied;
        keyboardController = new KeyboardController(this, player);
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

    private void SpawnFireball(Vector2 position, Direction direction)
    {
        if (gameEntities.Projectiles.OfType<Fireball>().Count(fireball => fireball.Active) >= MaxFireBall)
            return;

        sounds.Play(SoundType.Fireball);
        float flip = direction == Direction.Left ? -1f : 1f;
        var velocity = new Vector2(500f * flip, 200f);
        gameEntities.Projectiles.Add(new Fireball(
            spriteFactory.Create("projectiles", "fireball"), // new animation contoller for each fireball
            position + new Vector2(25 * flip, -20f),
            velocity,
            2f,
            400f
        ));
    }

    private void SpawnItem(ItemType type, Vector2 position)
    {
        Item item = new Item(spriteFactory, type, position);
        item.StartEmerging();
        item.Collected += OnItemCollected;
        gameEntities.Items.Add(item);

        if (type != ItemType.Coin)
        {
            sounds.Play(SoundType.Item);
        }
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

    private void SpawnHammer(Vector2 position, Direction direction)
    {
        float directionSign = direction == Direction.Left ? -1f : 1f;

        gameEntities.Projectiles.Add(new Hammer(
            spriteFactory.Create("projectiles", "hammer"),
            position + new Vector2(20f * directionSign, -20f),
            new Vector2(250f * directionSign, -350f),
            3f));
    }

    private void SpawnFire(Vector2 position, Direction direction)
    {
        float directionSign = direction == Direction.Left ? -1f : 1f;

        gameEntities.Projectiles.Add(new Fire(
            spriteFactory.Create("projectiles", "fire"),
            position + new Vector2(20f * directionSign, 0f),
            new Vector2(250f * directionSign, 0f),
            3f));
    }

    private void SpawnEgg(Vector2 position, Direction direction)
    {
        Egg egg = new Egg(
            spriteFactory.Create("projectiles", "egg"),
            position,
            new Vector2(0f, 0f),
            3f);
        egg.SummonSpiny += ReplaceEggWithSpiny;
        gameEntities.Projectiles.Add(egg);
    }

    private void SpawnBill(Vector2 position, Direction direction)
    {
        float directionSign = direction == Direction.Left ? -1f : 1f;
        entityBuffer.Enemies.Add(new BulletBill(
            spriteFactory.Create("enemies", "bulletBill"),
            position,
            new Vector2(250f * directionSign, 0f)));
    }

    private void ReplaceParaKoopaWithTurtle(ParaKoopa paraKoopa)
    {
        int index = gameEntities.Enemies.IndexOf(paraKoopa);
        if (index < 0)
            throw new InvalidOperationException(
                "Cannot replace a ParaKoopa that is not in the active enemy list.");

        gameEntities.Enemies[index] = new Turtle(
            spriteFactory.Create("enemies", "turtle"),
            paraKoopa.Position)
        {
            Velocity = paraKoopa.Velocity,
        };
    }

    private void ReplaceEggWithSpiny(Egg egg)
    {
        gameEntities.Enemies.Add(new Spiny(
            spriteFactory.Create("enemies", "spiny"),
            egg.Position));
    }



    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        KeyboardState keyboardState = Keyboard.GetState();

        if (currentScreen == GameScreen.Start)
        {
            startScreenState.Update(gameTime);

            if (startScreenState.StartRequested)
            {
                currentScreen = GameScreen.Playing;
            }

            previousKeyboardState = keyboardState;
            base.Update(gameTime);
            return;
        }
        if (currentScreen == GameScreen.GameOver)
        {
            gameOverScreenState.Update(gameTime);

            if (gameOverScreenState.RestartRequested)
            {
                ResetGame();
                currentScreen = GameScreen.Playing;
            }

            previousKeyboardState = keyboardState;
            base.Update(gameTime);
            return;
        }
        if (currentScreen == GameScreen.Playing &&
            keyboardState.IsKeyDown(Keys.P) &&
            previousKeyboardState.IsKeyUp(Keys.P))
        {
            currentScreen = GameScreen.Paused;
            pausedScreenState.ResetInput();
        }

        previousKeyboardState = keyboardState;

        if (currentScreen == GameScreen.Paused)
        {
            pausedScreenState.Update(gameTime);

            if (pausedScreenState.ResumeRequested)
            {
                currentScreen = GameScreen.Playing;
            }

            base.Update(gameTime);
            return;
        }

        keyboardController.Update(gameTime);
        mouseController.Update(gameTime);

        gameEntities.Update(gameTime);
        physicsEngine.Step(gameEntities, gameTime);
        stats.Update(gameTime);
        if (stats.IsOutOfTime)
        {
            foreach (IPlayer player in gameEntities.Players)
            {
                player.SetPower(MarioPower.Dead);
            }
        }
        gameEntities.Enemies.AddRange(entityBuffer.Enemies);
        entityBuffer.Enemies.Clear();

        foreach (IPlayer player in gameEntities.Players)
        {
            player.UpdateAnimation(gameTime);
        }
        camera.Follow(gameEntities.Players[0].Position, GraphicsDevice.Viewport.Width, levelWidth);

        if (reloadRoomRequested)
        {
            reloadRoomRequested = false;

            if (stats.Lives <= 0)
            {
                currentScreen = GameScreen.GameOver;
                gameOverScreenState.ResetInput();
            }
            else
            {
                LoadRoom(currentRoom);
            }
        }
        if (warpRequested)
        {
            warpRequested = false;
            Warp();
        }

        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        if (currentScreen == GameScreen.Start)
        {
            startScreenState.Draw(gameTime);
            base.Draw(gameTime);
            return;
        }
        if (currentScreen == GameScreen.GameOver)
        {
            gameOverScreenState.Draw(gameTime);
            base.Draw(gameTime);
            return;
        }
        GraphicsDevice.Clear(Color.CornflowerBlue);

        spriteBatch.Begin(
            SpriteSortMode.Deferred,
            null,
            SamplerState.PointClamp,
            null,
            null,
            null,
            camera.Transform);

        // Water fills the underwater room below the surface, behind everything else
        if (currentRoom == UnderwaterRoom)
        {
            spriteBatch.Draw(pixel, new Rectangle(0, WaterTopY, levelWidth, levelHeight - WaterTopY), WaterColor);
        }

        // Layers are drawn in enum order: Background, Item, Block, Foreground, Effects
        foreach (IGameObject.DrawLayer layer in Enum.GetValues<IGameObject.DrawLayer>())
        {
            foreach (IGameObject gameObj in gameEntities.GetAllGameObjects())
            {
                if (gameObj.layer == layer)
                {
                    gameObj.Draw(spriteBatch);
                }
            }
        }

        if (showHitboxes)
        {
            foreach (ICollider collider in gameEntities.GetAllColliders())
            {
                hitboxes.DrawHitbox(spriteBatch, collider.Bounds.Left, collider.Bounds.Right, collider.Bounds.Top, collider.Bounds.Bottom);
            }
        }

        spriteBatch.End();

        // HUD is drawn without the camera transform so it stays fixed on screen
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        hud.Draw(spriteBatch, GraphicsDevice.Viewport.Width);
        spriteBatch.End();
        if (currentScreen == GameScreen.Paused)
        {
            pausedScreenState.Draw(gameTime);
        }

        base.Draw(gameTime);
    }

}
