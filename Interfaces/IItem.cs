using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Interfaces.Player;

namespace Mario.Interfaces;

public interface IItem: IGameObject
{
    Vector2 Position { get; }
    Rectangle Bounds { get; }
    bool IsCollected { get; }
    void Collect(IPlayer player);
}
