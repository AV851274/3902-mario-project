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
    private IItem item;
    private IBlock block;
    private IController keyboardController;
    private IController mouseController;
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
        
        SpriteFactory spriteFactory = new SpriteFactory();
        spriteFactory.LoadTextures(Content);

        item = new Item(spriteFactory.CreateItemSprites(), new Vector2(400, 200));
        block = new Block(spriteFactory.CreateBlockSprites(), new Vector2(250, 200));
        playerSprite = spriteFactory.CreatePlayerSprite();

        keyboardController = new KeyboardController(this, player, item, block);

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        keyboardController.Update(gameTime);
        mouseController.Update(gameTime);
        player.Update(gameTime);
        block.Update(gameTime);
        item.Update(gameTime);

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
        block.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
