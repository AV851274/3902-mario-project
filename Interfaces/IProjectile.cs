using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Interfaces;

public interface IProjectile: IGameObject
{
    Vector2 Position { get; }
    public bool Active { get; }
}
