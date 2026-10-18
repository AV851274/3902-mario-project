using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Interfaces;

public interface IBlock: IGameObject
{
    Vector2 Position { get; }
    Rectangle Bounds { get; }
    void Hit();
    
}
