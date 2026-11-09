using System.Collections.Generic;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.State.GameScreen;

public sealed class GameScreenManager
{
    private readonly Dictionary<GameScreenType, IGameScreenState> states;
    private IGameScreenState currentState;

    public GameScreenManager(
        SpriteBatch spriteBatch,
        SpriteFont screenFont,
        GraphicsDevice graphicsDevice,
        GameWorld world)
    {
        states = new()
        {
            [GameScreenType.Start] = new StartScreenState(spriteBatch, screenFont, graphicsDevice),
            [GameScreenType.Playing] = new PlayingScreenState(
                spriteBatch, screenFont, graphicsDevice, world),
            [GameScreenType.Paused] = new PausedScreenState(
                spriteBatch, screenFont, graphicsDevice, world),
            [GameScreenType.GameOver] = new GameOverScreenState(
                spriteBatch, screenFont, graphicsDevice, world),
            [GameScreenType.CourseClear] = new CourseClearScreenState(
                spriteBatch, screenFont, graphicsDevice, world),
        };

        currentState = states[GameScreenType.Start];
        currentState.Enter();
    }

    private void ChangeTo(GameScreenType screen)
    {
        IGameScreenState nextState = states[screen];
        if (ReferenceEquals(currentState, nextState))
            return;

        currentState.Exit();
        currentState = nextState;
        currentState.Enter();
    }

    public void Update(GameTime gameTime)
    {
        GameScreenType? nextScreen = currentState.Update(gameTime);
        if (nextScreen.HasValue)
            ChangeTo(nextScreen.Value);
    }

    public void Draw(GameTime gameTime) => currentState.Draw(gameTime);
}
