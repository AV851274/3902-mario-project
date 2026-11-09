using Microsoft.Xna.Framework;

namespace Mario.Game.Interfaces;

public interface IGameScreenState
{
    void Enter();
    void Exit();
    void Update(GameTime gameTime);
    void Draw(GameTime gameTime);
}
