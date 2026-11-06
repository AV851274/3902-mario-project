using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Game.Interfaces.Player;

namespace Mario.Game.Interfaces;

public interface IItem: IGameObject, ICollider
{
    event Action CoinCollected;
    Vector2 Position { get; }
    bool IsCollected { get; }
    void Collect(IPlayer player);
}
