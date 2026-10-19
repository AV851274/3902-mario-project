using Mario.Game.Physics;
using Microsoft.Xna.Framework;

namespace Mario.Game.Interfaces;

public interface ICollider
{
    public enum BlockingModes
    {
        None,
        Solid,
        TopOnly,
    }

    Box Bounds { get; }
    bool CollisionEnabled => true;
    BlockingModes BlockingMode { get; }
    void OnCollision(CollisionEvent collision) { }
}
