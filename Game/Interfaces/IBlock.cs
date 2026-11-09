using System;
using Mario.Game.Audio;
using Microsoft.Xna.Framework;

namespace Mario.Game.Interfaces;

public interface IBlock: IGameObject, ICollider
{
    Vector2 Position { get; }
    event Action<SoundType> SoundRequested;
    void Hit(bool big);
    bool IsWarpPipe { get; }

}
