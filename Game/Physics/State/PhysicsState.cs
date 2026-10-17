using Microsoft.Xna.Framework;

namespace Mario.Physics.State;

public struct PhysicsState
{
    public PhysicsState(Vector2 position, Vector2 velocity, bool isOnGround = false)
    {
        Position = position;
        Velocity = velocity;
        IsOnGround = isOnGround;
    }

    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public bool IsOnGround { get; set; }
}
