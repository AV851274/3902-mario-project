using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mario.Game.State;

public class GameOverScreenState : GameScreenStateBase
{
    private KeyboardState previousKeyboardState;

    public bool RestartRequested { get; private set; }

    public GameOverScreenState(
        SpriteBatch spriteBatch,
        SpriteFont screenFont,
        GraphicsDevice graphicsDevice)
        : base(spriteBatch, screenFont, graphicsDevice)
    {
    }

    public void ResetInput()
    {
        previousKeyboardState = Keyboard.GetState();
        RestartRequested = false;
    }

    public override void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Enter) &&
            previousKeyboardState.IsKeyUp(Keys.Enter))
        {
            RestartRequested = true;
        }

        previousKeyboardState = keyboardState;
    }

    public override void Draw(GameTime gameTime)
    {
        graphicsDevice.Clear(Color.Black);

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        float centerY = graphicsDevice.Viewport.Height / 2f;

        DrawCenteredText("GAME OVER", centerY - 80, Color.White);
        DrawCenteredText("PRESS ENTER TO RESTART", centerY + 40, Color.White);

        spriteBatch.End();
    }
}