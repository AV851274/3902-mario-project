using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mario.Game.State.GameScreen;

public class GameOverScreenState : GameScreenStateBase
{
    private readonly GameWorld world;

    public GameOverScreenState(
        SpriteBatch spriteBatch,
        SpriteFont screenFont,
        GraphicsDevice graphicsDevice,
        GameWorld world)
        : base(spriteBatch, screenFont, graphicsDevice)
    {
        this.world = world;
    }

    public override GameScreenType? Update(GameTime gameTime)
    {
        if (WasKeyPressed(Keys.Enter))
        {
            world.ResetGame();
            return GameScreenType.Playing;
        }

        return null;
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
