using Microsoft.Xna.Framework;

namespace Mario.Game.Interfaces;

public interface IPhysicsBody : ICollider
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 PendingAcceleration { get; set; }
    public bool IsOnGround { get; set; }
    public float GravityScale => 1f;
    float MaxFallSpeed => float.PositiveInfinity;
    float MaxHorizontalSpeed => float.PositiveInfinity;
}
