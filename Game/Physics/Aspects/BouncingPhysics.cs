using System;

using Mario.Game.Physics.Interfaces;
using Mario.Game.Physics.State;

namespace Mario.Game.Physics.Aspects;

public sealed class BouncingPhysics : IPhysicsAspect
{
    private readonly float groundY;
    private readonly float bounceSpeed;

    public float BounceHeight { get; }

    public BouncingPhysics(float bounceHeight, float groundY = 384f, float gravity = 1000f)
    {
        if (gravity <= 0f)
            throw new ArgumentOutOfRangeException(nameof(gravity));
        if (bounceHeight < 0f)
            throw new ArgumentOutOfRangeException(nameof(bounceHeight));

        this.groundY = groundY;
        BounceHeight = bounceHeight;
        bounceSpeed = MathF.Sqrt(2f * gravity * bounceHeight);
    }

    public void Apply(ref PhysicsState state, float deltaTime)
    {
        if (state.Position.Y >= groundY)
        {
            state.Position = new Microsoft.Xna.Framework.Vector2(state.Position.X, groundY);
            state.Velocity = new Microsoft.Xna.Framework.Vector2(state.Velocity.X, -bounceSpeed);
            state.IsOnGround = false;
        }
    }
}