using Mario.Game.Interfaces.Player;
using Mario.Game.Animation;
using Mario.Game.Controllers;
using Mario.Game.Entities.Enemies;
using Mario.Game.Interfaces;
using Mario.Game.Entities.Players;
using Mario.Game.Entities.Projectiles;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Mario.Game.State;
using Mario.Game.Levels;
using Mario.Game.Rendering.Camera;
using Mario.Game.Physics;

namespace Mario.Game;

public class MarioGame : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private IPlayer player;
    private EnemyCycler enemies;


    // private ISprite fireballSprite;
    private IController keyboardController;
    private IController mouseController;
    private SpriteFactory spriteFactory;
    private GameEntities gameEntities = new GameEntities();
    private Camera2D camera = new Camera2D();
    private int levelWidth;

    public MarioGame()
    {
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
        gameEntities.Reset();
        camera = new Camera2D();
        // fireballSprite = spriteFactory.CreateFireBallSprite();

        var level = LevelLoader.LoadLevel(Content, gameEntities, spriteFactory);

        Vector2 marioSpawn = level.spawn;
        levelWidth = level.width;
        enemies = new EnemyCycler(gameEntities.Enemies);

        // Hammer Bros announce their throws; spawn the hammer projectile when they do
        foreach (IEnemy enemy in gameEntities.Enemies)
        {
            if (enemy is HammerBro hammerBro)
            {
                hammerBro.SummonHammer += SpawnHammer;
            }
        }

        gameEntities.Players.Add(CreateMario(false, marioSpawn));
    }


    // Keys 1-6: switch between regular and swimming Mario, then set his size
    public void ChangeMario(bool swimming, MarioPower power)
    {
        player.SetSwimming(swimming);
        player.SetPower(power);
    }

    private IPlayer CreateMario(bool swimming, Vector2 position)
    {
        player = new Player(spriteFactory, position);
        player.SetSwimming(swimming);
        player.SummonFireball += SpawnFireball;
        keyboardController = new KeyboardController(this, player, enemies);
        mouseController = new MouseController(player);
        return player;
    }

    private void SpawnFireball(Vector2 position, Direction direction)
    {
        float flip = direction == Direction.Left ? -1f : 1f;
        var velocity = new Vector2(850f * flip, 0);
        gameEntities.Projectiles.Add(new Fireball(
            spriteFactory.CreateFireballSprite(), // new animation contoller for each fireball
            position + new Vector2(25 * flip, -50f),
            velocity,
            1f,
            1000f,
            400f
        ));
    }

    private void SpawnHammer(Vector2 position, Direction direction)
    {
        float directionSign = direction == Direction.Left ? -1f : 1f;

        gameEntities.Projectiles.Add(new Hammer(
            spriteFactory.CreateHammerSprite(),
            position + new Vector2(20f * directionSign, -20f),
            new Vector2(250f * directionSign, -350f),
            3f,
            1000f,
            600f
        ));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        keyboardController.Update(gameTime);
        //mouseController.Update(gameTime);

        // Save Mario's position before moving
        Rectangle previousBounds = player.Bounds;

        // Update Mario's movement and gravity
        player.Update(gameTime);

        // Resolve collisions with walls
        CollisionManager.ResolvePlayerWallCollisions(player, gameEntities.Blocks, previousBounds);
        // Resolve collisions with ceilings
        CollisionManager.ResolvePlayerCeilingCollisions(player, gameEntities.Blocks, previousBounds);
        // Resolve collisions with blocks
        CollisionManager.ResolvePlayerBlockCollisions(player, gameEntities.Blocks, previousBounds);
        // Resolve collisions with items
        CollisionManager.ResolvePlayerItemCollisions(player, gameEntities.Items);
        gameEntities.Items.RemoveAll(item => item.IsCollected);

        player.UpdateAnimation(gameTime);

        // Update camera
        camera.Follow(player.Position, GraphicsDevice.Viewport.Width, levelWidth);

        foreach (IItem currentItem in gameEntities.Items)
        {
            Rectangle previousItemBounds = currentItem.Bounds;
            currentItem.Update(gameTime);
            CollisionManager.ResolveItemWallCollisions(currentItem, gameEntities.Blocks, previousItemBounds);
        }

        foreach (IBlock currentBlock in gameEntities.Blocks)
        {
            currentBlock.Update(gameTime);
        }

        foreach (IEnemy enemy in gameEntities.Enemies)
        {
            Rectangle previousEnemyBounds = enemy.Bounds;
            enemy.Update(gameTime);
            CollisionManager.ResolveEnemyBlockCollisions(enemy, gameEntities.Blocks, previousEnemyBounds);
        }

        // Stomp enemies Mario lands on, take damage from the rest
        CollisionManager.ResolvePlayerEnemyCollisions(player, gameEntities.Enemies, previousBounds);

        // Defeated enemies fall off the bottom of the level; remove them once they're far below
        gameEntities.Enemies.RemoveAll(enemy => enemy.Position.Y > 1000f);

        UpdateProjectiles(gameTime);

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

    private void UpdateProjectiles(GameTime gameTime)
    {
        foreach (var projectile in gameEntities.Projectiles)
        {
            if (projectile.Active)
                projectile.Update(gameTime);
        }

        gameEntities.Projectiles.RemoveAll(p => !p.Active);
    }
}