using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Game2D.Animation;
using Game2D.Interfaces;

namespace Monogame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private IPlayer player;
    private ISprite playerSprite;
    private ISprite fireballSprite;
    private IItem item;
    private IBlock block;
    private EnemyCycler enemies;
    private IController keyboardController;
    private IController mouseController;
    private SpriteFactory spriteFactory;
    private List<IProjectile> projectiles = [];

    public Game1()
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

    //TODO: I THINK THIS IS GOING TO NEED TO MOVE SOON ResetGame()
    public void ResetGame()
    {
        var player1 = new Player(new Vector2(100, 100));
        player1.SummonFireball +=
            (position, direction) => {
                float flip = direction == Direction.Left ? -1f : 1f;
                var velocity = new Vector2(850f * flip, 0);
                projectiles.Add(
                    new Fireball(fireballSprite, position + new Vector2(20 * flip, 2f), velocity, 1f));
            };
        this.player = player1;


        playerSprite = spriteFactory.CreatePlayerSprite();
        fireballSprite = spriteFactory.CreateFireBallSprite();
        projectiles.Clear();

        item = new Item(spriteFactory.CreateItemSprites(), new Vector2(400, 200));
        block = new Block(spriteFactory.CreateBlockSprites(), new Vector2(250, 200));
        enemies = new EnemyCycler(new List<IEnemy> {
            new Goomba(spriteFactory.CreateGoombaSprite(), new Vector2(600, 408)),
            new Turtle(spriteFactory.CreateTurtleSprite(), new Vector2(600, 392))
        });

        keyboardController = new KeyboardController(this, player, item, block, enemies);
        mouseController = new MouseController(player);
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

        if (!player.IsOnGround)
        {
            playerSprite.Play("Jump");
        }
        else if (player.IsMoving)
        {
            playerSprite.Play("Run");
        }
        else
        {
            playerSprite.Play("Idle");
        }

        playerSprite.UpdateAnimation(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteEffects spriteEffect = player.FacingDirection == Direction.Right
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;

        _spriteBatch.Begin();

        playerSprite.Draw(_spriteBatch, player.Position, effects: spriteEffect);

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