using System;
using Mario.Game.Entities.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Interfaces;

public interface IBlock: IGameObject, ICollider
{
    Vector2 Position { get; }
    event Action<ItemType, Vector2> ItemReleased;
    void Hit(bool big);
}
