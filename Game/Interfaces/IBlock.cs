using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Interfaces;

public interface IBlock: IGameObject, ICollider
{
    Vector2 Position { get; }
    void Hit();
}
