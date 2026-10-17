using System;

using Mario.Physics.Interfaces;
using Mario.Physics.State;

namespace Mario.Physics.Aspects;

public sealed class GravityPhysics : IPhysicsAspect
{
    private readonly float gravity;
    private readonly float maxFallSpeed;

    public bool Enabled { get; set; } = true;

    public GravityPhysics(float gravity, float maxFallSpeed = 0f)
    {
        if (gravity < 0f)
            throw new ArgumentOutOfRangeException(nameof(gravity));
        if (maxFallSpeed < 0f)
            throw new ArgumentOutOfRangeException(nameof(maxFallSpeed));

        this.gravity = gravity;
        this.maxFallSpeed = maxFallSpeed;
    }

    public void Apply(ref PhysicsState state, float deltaTime)
    {
        if (!Enabled)
            return;

        float velocityY = state.Velocity.Y + gravity * deltaTime;
        if (maxFallSpeed > 0f && velocityY > maxFallSpeed)
        {
            velocityY = maxFallSpeed;
        }

        state.Velocity = new Microsoft.Xna.Framework.Vector2(state.Velocity.X, velocityY);
    }
}
