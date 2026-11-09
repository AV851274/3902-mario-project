using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mario.Game.State.GameScreen;

public class GameOverScreenState : GameScreenStateBase
{
    private readonly GameWorld world;

    public GameOverScreenState(
        GameScreenManager screens,
        SpriteBatch spriteBatch,
        SpriteFont screenFont,
        GraphicsDevice graphicsDevice,
        GameWorld world)
        : base(screens, spriteBatch, screenFont, graphicsDevice)
    {
        this.world = world;
    }

    public override void Update(GameTime gameTime)
    {
        if (WasKeyPressed(Keys.Enter))
        {
            world.ResetGame();
            screens.ChangeTo(GameScreenType.Playing);
        }
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
