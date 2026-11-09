using Mario.Game.Assets;
using Mario.Game.Rendering;
using Mario.Game.Rendering.Sprites;
using Mario.Game.State;
using Mario.Game.State.GameScreen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mario.Game;

public class MarioGame : Microsoft.Xna.Framework.Game
{
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch = null!;
    private GameWorld world = null!;
    private GameScreenManager screens = null!;

    public MarioGame()
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 768;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        var assets = new AssetManager(Content, "definitions/assets.json");
        var stats = new GameStats();
        var renderer = new WorldRenderer(
            GraphicsDevice, spriteBatch, new HitboxRenderer(assets), new Hud(assets.GetFont("hud"), stats));
        world = new GameWorld(this, assets, stats, renderer);
        screens = new GameScreenManager(spriteBatch, assets.GetFont("hud"), GraphicsDevice, world);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
            return;
        }

        screens.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        screens.Draw(gameTime);
        base.Draw(gameTime);
    }

    protected override void UnloadContent()
    {
        world.Dispose();
        spriteBatch.Dispose();
        base.UnloadContent();
    }
}
