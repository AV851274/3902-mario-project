using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mario.Game.State;

public class StartScreenState : GameScreenStateBase
{
    private KeyboardState previousKeyboardState;

    public bool StartRequested { get; private set; }

    public StartScreenState(
        SpriteBatch spriteBatch,
        SpriteFont screenFont,
        GraphicsDevice graphicsDevice)
        : base(spriteBatch, screenFont, graphicsDevice)
    {
    }

    public override void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Enter) &&
            previousKeyboardState.IsKeyUp(Keys.Enter))
        {
            StartRequested = true;
        }

        previousKeyboardState = keyboardState;
    }

    public override void Draw(GameTime gameTime)
    {
        graphicsDevice.Clear(Color.Black);

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        float centerY = graphicsDevice.Viewport.Height / 2f;

        DrawCenteredText("SUPER MARIO BROS", centerY - 80, Color.White);
        DrawCenteredText("PRESS ENTER TO START", centerY + 40, Color.White);

        spriteBatch.End();
    }
}