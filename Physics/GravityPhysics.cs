using Microsoft.Xna.Framework;

public static class GravityPhysics
{
    public static bool Apply(
        ref Vector2 position,
        ref Vector2 velocity,
        float deltaTime,
        float gravity,
        float groundY,
        float? maxFallSpeed = null)
    {
        velocity.Y += gravity * deltaTime;
        if (maxFallSpeed.HasValue && velocity.Y > maxFallSpeed.Value)
        {
            velocity.Y = maxFallSpeed.Value;
        }

        position += velocity * deltaTime;

        if (position.Y < groundY)
        {
            return false;
        }

        position.Y = groundY;
        velocity.Y = 0f;
        return true;
    }
}