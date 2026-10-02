using Mario.Animation;
using Mario.Blocks;
using Mario.Controllers;
using Mario.Enemies;
using Mario.Interfaces;
using Mario.Items;
using Mario.Players;
using Mario.Projectiles;
using Mario.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Mario;

public class MarioGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private IPlayer player;

    private bool isSwimming;

    // private ISprite fireballSprite;
    private IItem item;
    private IBlock block;
    private EnemyCycler enemies;
    private IController keyboardController;
    private IController mouseController;
    private SpriteFactory spriteFactory;
    private List<IProjectile> projectiles = [];

    public MarioGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        spriteFactory = new SpriteFactory();
        spriteFactory.LoadTextures(Content);

        ResetGame();
    }

    public void ResetGame()
    {
        // fireballSprite = spriteFactory.CreateFireBallSprite();
        projectiles.Clear();

        item = new Item(spriteFactory.CreateItemSprites(), new Vector2(400, 200));
        block = new Block(spriteFactory.CreateBlockSprites(), new Vector2(250, 200));
        enemies = new EnemyCycler(new List<IEnemy> {
            new Goomba(spriteFactory.CreateGoombaSprite(), new Vector2(600, 408)),
            new Turtle(spriteFactory.CreateTurtleSprite(), new Vector2(600, 392))
        });

        CreateMario(false, new Vector2(100, 100));
    }

    // Keys 1-6: switch between regular and swimming Mario, then set his size
    public void ChangeMario(bool swimming, MarioPower power)
    {
        if (swimming != isSwimming)
        {
            CreateMario(swimming, player.Position);
        }

        switch (power)
        {
            case MarioPower.Small: player.BecomeSmall(); break;
            case MarioPower.Big: player.BecomeBig(); break;
            case MarioPower.Fire: player.BecomeFire(); break;
        }
    }

    private void CreateMario(bool swimming, Vector2 position)
    {
        isSwimming = swimming;
        player = swimming
            ? new SwimmingPlayer(spriteFactory, position)
            : new Player(spriteFactory, position);
        player.SummonFireball += SpawnFireball;

        // Controllers hold on to the player, so they're rebuilt whenever Mario is replaced
        keyboardController = new KeyboardController(this, player, item, block, enemies);
        mouseController = new MouseController(player);
    }

    private void SpawnFireball(Vector2 position, Direction direction)
    {
        float flip = direction == Direction.Left ? -1f : 1f;
        var velocity = new Vector2(850f * flip, 0);
        projectiles.Add(new Fireball(
            spriteFactory.CreateFireBallSprite(), // new animation contoller for each fireball
            position + new Vector2(25 * flip, -25f),
            velocity,
            1f,
            1000f,
            400f
        ));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        keyboardController.Update(gameTime);
        //mouseController.Update(gameTime);
        player.Update(gameTime);
        block.Update(gameTime);
        enemies.Update(gameTime);
        item.Update(gameTime);
        UpdateProjectiles(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        player.Draw(_spriteBatch);

        item.Draw(_spriteBatch);
        enemies.Draw(_spriteBatch);
        block.Draw(_spriteBatch);
        foreach (var projectile in projectiles)
        {
            projectile.Draw(_spriteBatch);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void UpdateProjectiles(GameTime gameTime)
    {
        foreach (var projectile in projectiles)
        {
            if (projectile.Active)
                projectile.Update(gameTime);
        }

        projectiles.RemoveAll(p => !p.Active);
    }
}
