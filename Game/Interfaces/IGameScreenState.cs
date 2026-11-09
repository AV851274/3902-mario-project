using Mario.Game.State.GameScreen;
using Microsoft.Xna.Framework;

namespace Mario.Game.Interfaces;

public interface IGameScreenState
{
    void Enter();
    void Exit();
    GameScreenType? Update(GameTime gameTime);
    void Draw(GameTime gameTime);
}
