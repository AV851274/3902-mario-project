using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mario.Game.State;

public class PausedScreenState : GameScreenStateBase
{
    private KeyboardState previousKeyboardState;

    public bool ResumeRequested { get; private set; }
    public PausedScreenState(
        SpriteBatch spriteBatch,
        SpriteFont screenFont,
        GraphicsDevice graphicsDevice)
        : base(spriteBatch, screenFont, graphicsDevice)
    {
    }
    public void ResetInput()
    {
        previousKeyboardState = Keyboard.GetState();
        ResumeRequested = false;
    }

    public override void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.P) &&
            previousKeyboardState.IsKeyUp(Keys.P))
        {
            ResumeRequested = true;
        }

        previousKeyboardState = keyboardState;
    }

    public override void Draw(GameTime gameTime)
    {
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        float centerY = graphicsDevice.Viewport.Height / 2f;
        float textHeight = screenFont.MeasureString("PAUSED").Y;

        DrawCenteredText(
            "PAUSED",
            centerY - textHeight / 2f,
            Color.White
        );

        spriteBatch.End();
    }
}