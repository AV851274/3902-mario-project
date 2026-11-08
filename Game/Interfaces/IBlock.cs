using System;
using Mario.Game.Audio;
using Mario.Game.Entities.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Interfaces;

public interface IBlock: IGameObject, ICollider
{
    Vector2 Position { get; }
    event Action<ItemType, Vector2> ItemReleased;
    event Action<SoundType> SoundRequested;
    void Hit(bool big);
    bool IsWarpPipe { get; }
    bool IsFlagPole { get; }

}
