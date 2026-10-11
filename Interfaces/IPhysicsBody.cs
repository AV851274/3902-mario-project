using Microsoft.Xna.Framework;

namespace Mario.Interfaces;

public interface IPhysicsBody
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public bool IsOnGround { get; set; }
    public bool HasGravity { get; }
}
