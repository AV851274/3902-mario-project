using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mario.Game.State.GameScreen;

public class PlayingScreenState : GameScreenStateBase
{
    private readonly GameWorld world;

    public PlayingScreenState(
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
            screens.ChangeTo(GameScreenType.Paused);
            return;
        }

        world.Update(gameTime);
        if (world.IsGameOver)
            screens.ChangeTo(GameScreenType.GameOver);
    }

    public override void Draw(GameTime gameTime) => world.Draw();
}
