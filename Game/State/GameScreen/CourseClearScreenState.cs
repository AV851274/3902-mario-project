using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mario.Game.State.GameScreen;

public class CourseClearScreenState : GameScreenStateBase
{
    private readonly GameWorld world;
    private KeyboardState previousKeyboardState;

    public CourseClearScreenState(
        GameScreenManager screens,
        SpriteBatch spriteBatch,
        SpriteFont screenFont,
        GraphicsDevice graphicsDevice,
        GameWorld world)
        : base(screens, spriteBatch, screenFont, graphicsDevice)
    {
        this.world = world;
    }

    public override void Enter()
    {
        base.Enter();
        previousKeyboardState = Keyboard.GetState();
    }

    public override void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        bool restart = (keyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter)) ||
                       (keyboardState.IsKeyDown(Keys.R) && previousKeyboardState.IsKeyUp(Keys.R));
        previousKeyboardState = keyboardState;
        if (restart)
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
        DrawCenteredText("COURSE CLEAR", centerY - 80, Color.White);
        DrawCenteredText("PRESS ENTER OR R TO RESTART", centerY + 40, Color.White);
        spriteBatch.End();
    }
}
