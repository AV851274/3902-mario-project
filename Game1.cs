using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Monogame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D playerTexture;

    private IPlayer player;
    private IItem item;
    private IController keyboardController;
    private IController mouseController;
    private int animationFrame = 0;
    private double animationTimer = 0;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        player = new Player(new Vector2(100, 100));
        mouseController = new MouseController(player);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        playerTexture = Content.Load<Texture2D>("mario");
        SpriteFactory spriteFactory = new SpriteFactory();
        spriteFactory.LoadTextures(Content);

        item = new Item(spriteFactory.CreateItemSprites(), new Vector2(400, 200));

        keyboardController = new KeyboardController(this, player, item);

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        keyboardController.Update(gameTime);
        mouseController.Update(gameTime);
        player.Update(gameTime);
        item.Update(gameTime);
        if (player.IsMoving)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer >= 0.15)
            {
                animationFrame++;

                if (animationFrame > 2)
                {
                    animationFrame = 0;
                }

                animationTimer = 0;
            }
        }
        else
        {
            animationFrame = 0;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        Rectangle sourceRectangle;
        SpriteEffects spriteEffect = SpriteEffects.None;

        if (!player.IsOnGround)
        {
            sourceRectangle = new Rectangle(150, 0, 14, 15);

            if (player.FacingDirection == 1)
            {
                spriteEffect = SpriteEffects.FlipHorizontally;
            }
        }
        else if (player.IsMoving)
        {
            if (animationFrame == 0)
            {
                sourceRectangle = new Rectangle(60, 0, 14, 16);
            }
            else if (animationFrame == 1)
            {
                sourceRectangle = new Rectangle(89, 0, 16, 16);
            }
            else
            {
                sourceRectangle = new Rectangle(121, 0, 12, 16);
            }

            if (player.FacingDirection == 1)
            {
                spriteEffect = SpriteEffects.FlipHorizontally;
            }
        }
        else
        {
            if (player.FacingDirection == -1)
            {
                sourceRectangle = new Rectangle(0, 57, 16, 22);
            }
            else
            {
                sourceRectangle = new Rectangle(389, 57, 16, 22);
            }
        }

        if (player.IsMoving && player.FacingDirection == 1)
        {
            spriteEffect = SpriteEffects.FlipHorizontally;
        }

        _spriteBatch.Begin();

        _spriteBatch.Draw(
            playerTexture,
            new Rectangle((int)player.Position.X,(int)player.Position.Y,40,40),
            sourceRectangle,
            Color.White,
            0f,
            Vector2.Zero,
            spriteEffect,
            0f
        );

        item.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
