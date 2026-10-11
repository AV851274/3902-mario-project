using Microsoft.Xna.Framework;

namespace Mario.Physics;

public static class GravityPhysics
{
    public static void Apply(
        ref Vector2 position,
        ref Vector2 velocity,
        float deltaTime,
        float gravity,
        float maxFallSpeed = 0f)
    {
        velocity.Y += gravity * deltaTime;

        if (maxFallSpeed > 0f && velocity.Y > maxFallSpeed)
        {
            velocity.Y = maxFallSpeed;
        }

        position += velocity * deltaTime;
    }
}