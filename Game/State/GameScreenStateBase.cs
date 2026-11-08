using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.State;

public abstract class GameScreenStateBase : IGameScreenState
{
    protected readonly SpriteBatch spriteBatch;
    protected readonly SpriteFont screenFont;
    protected readonly GraphicsDevice graphicsDevice;

    protected GameScreenStateBase(
        SpriteBatch spriteBatch,
        SpriteFont screenFont,
        GraphicsDevice graphicsDevice)
    {
        this.spriteBatch = spriteBatch;
        this.screenFont = screenFont;
        this.graphicsDevice = graphicsDevice;
    }

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime);

    protected void DrawCenteredText(string text, float y, Color color)
    {
        Vector2 textSize = screenFont.MeasureString(text);

        float x = (graphicsDevice.Viewport.Width - textSize.X) / 2f;

        spriteBatch.DrawString(
            screenFont,
            text,
            new Vector2(x, y),
            color
        );
    }
}