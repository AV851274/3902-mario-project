using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Interfaces;

public interface IBlock: IGameObject
{
    Vector2 Position { get; }

    //TODO: Collision next sprint, Bump(), Break(), etc.)
}
