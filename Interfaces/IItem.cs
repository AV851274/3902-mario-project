using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Interfaces;

public interface IItem: IGameObject
{
    Vector2 Position { get; }
}
