using Microsoft.Xna.Framework;
using Mario.Game.Interfaces.Player;

namespace Mario.Game.Interfaces;

public interface IItem: IGameObject, ICollider
{
    Vector2 Position { get; }
    bool IsCollected { get; }
    void Collect(IPlayer player);
}
