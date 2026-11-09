using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mario.Game.State.GameScreen;

public class PlayingScreenState : GameScreenStateBase
{
    private readonly GameWorld world;

    public PlayingScreenState(
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
        if (WasKeyPressed(Keys.P))
        {
            return GameScreenType.Paused;
        }

        world.Update(gameTime);
        if (world.IsCourseClear)
            return GameScreenType.CourseClear;
        if (world.IsGameOver)
            return GameScreenType.GameOver;

        return null;
    }

    public override void Draw(GameTime gameTime) => world.Draw();
}
