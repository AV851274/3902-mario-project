using Mario.Game.Physics;
using Microsoft.Xna.Framework;

namespace Mario.Game.Interfaces;

public interface ICollider
{
    Box Bounds { get; }
    bool CollisionEnabled => true;
    bool IsSolid => false;
    void OnCollision(in CollisionEvent collision) { }
}
