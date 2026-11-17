using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mario.Game.State.GameScreen;

public class StartScreenState : GameScreenStateBase
{
    public StartScreenState(
        SpriteBatch spriteBatch,
        SpriteFont screenFont,
        GraphicsDevice graphicsDevice)
        : base(spriteBatch, screenFont, graphicsDevice)
    {
    }

    public override GameScreenType? Update(GameTime gameTime)
    {
        if (WasKeyPressed(Keys.Enter))
        {
            return GameScreenType.Playing;
        }

        return null;
    }

    public override void Draw(GameTime gameTime)
    {
        graphicsDevice.Clear(Color.Black);

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        float centerY = graphicsDevice.Viewport.Height / 2f;

        DrawCenteredText("SUPER MARIO BROS", centerY - GameConstants.Screen.TitleOffset, Color.White);
        DrawCenteredText("PRESS ENTER TO START", centerY + GameConstants.Screen.PromptOffset, Color.White);

        spriteBatch.End();
    }
}
