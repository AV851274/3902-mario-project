using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mario.Game.State.GameScreen;

public class PausedScreenState : GameScreenStateBase
{
    private readonly GameWorld world;

    public PausedScreenState(
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
        if (WasKeyPressed(Keys.P))
        {
            screens.ChangeTo(GameScreenType.Playing);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        world.Draw();

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
