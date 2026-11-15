using Microsoft.Xna.Framework;

namespace Mario.Game.Interfaces;

public interface IBlock: IGameObject, ICollider
{
    Vector2 Position { get; }
    void Hit(bool big);
    bool IsWarpPipe { get; }
    bool IsFlagPole { get; }

}
