using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mario.Game.State.GameScreen;

public abstract class GameScreenStateBase : IGameScreenState
{
    private KeyboardState previousKeyboardState;
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

    // Synchronize input on every entry, including when a cached state is reused.
    public virtual void Enter() => previousKeyboardState = Keyboard.GetState();

    public virtual void Exit() { }

    protected bool WasKeyPressed(Keys key)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        bool pressed = keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        previousKeyboardState = keyboardState;
        return pressed;
    }

    public abstract GameScreenType? Update(GameTime gameTime);

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
