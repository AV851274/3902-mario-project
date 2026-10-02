using Microsoft.Xna.Framework;

public static class BouncingPhysics
{
    public static void Apply(
        ref Vector2 position,
        ref Vector2 velocity,
        float deltaTime,
        float gravity,
        float groundY,
        float bounceSpeed)
    {
        velocity.Y += gravity * deltaTime;
        position += velocity * deltaTime;

        if (position.Y >= groundY)
        {
            position.Y = groundY;
            velocity.Y = -bounceSpeed;
        }
    }
}