using System;

using Mario.Game.Physics.Interfaces;
using Mario.Game.Physics.State;

namespace Mario.Game.Physics.Aspects;

public sealed class HorizontalMovementPhysics : IPhysicsAspect
{
    private readonly float moveAcceleration;
    private readonly float stopAcceleration;
    private readonly float maxSpeed;
    private readonly float turnAcceleration;
    private readonly float dashSpeedMultiplier;
    private readonly float dashAccelerationMultiplier;
    private readonly float maxGroundSpeed;

    private float direction;
    private bool isDashing;
    private bool shouldStop;

    public bool IsSkidding { get; private set; }

    public HorizontalMovementPhysics(
        float moveAcceleration,
        float stopAcceleration,
        float maxSpeed,
        float turnAcceleration = 0f,
        float dashSpeedMultiplier = 1f,
        float dashAccelerationMultiplier = 1f,
        float maxGroundSpeed = 0f)
    {
        if (moveAcceleration < 0f)
            throw new ArgumentOutOfRangeException(nameof(moveAcceleration));
        if (stopAcceleration < 0f)
            throw new ArgumentOutOfRangeException(nameof(stopAcceleration));
        if (maxSpeed < 0f)
            throw new ArgumentOutOfRangeException(nameof(maxSpeed));
        if (turnAcceleration < 0f)
            throw new ArgumentOutOfRangeException(nameof(turnAcceleration));
        if (dashSpeedMultiplier < 0f)
            throw new ArgumentOutOfRangeException(nameof(dashSpeedMultiplier));
        if (dashAccelerationMultiplier < 0f)
            throw new ArgumentOutOfRangeException(nameof(dashAccelerationMultiplier));
        if (maxGroundSpeed < 0f)
            throw new ArgumentOutOfRangeException(nameof(maxGroundSpeed));

        this.moveAcceleration = moveAcceleration;
        this.stopAcceleration = stopAcceleration;
        this.maxSpeed = maxSpeed;
        this.turnAcceleration = turnAcceleration;
        this.dashSpeedMultiplier = dashSpeedMultiplier;
        this.dashAccelerationMultiplier = dashAccelerationMultiplier;
        this.maxGroundSpeed = maxGroundSpeed;
    }

    public void SetInput(float direction, bool isDashing, bool shouldStop)
    {
        this.direction = Math.Clamp(direction, -1f, 1f);
        this.isDashing = isDashing;
        this.shouldStop = shouldStop;
    }

    public void Apply(ref PhysicsState state, float deltaTime)
    {
        IsSkidding = false;
        float speed = state.Velocity.X;
        if (shouldStop || direction == 0f)
        {
            float change = stopAcceleration * deltaTime;
            speed = speed > 0f
                ? Math.Max(0f, speed - change)
                : Math.Min(0f, speed + change);
        }
        else
        {
            float sign = Math.Sign(direction);
            IsSkidding = state.IsOnGround && speed * sign < 0f;
            float acceleration = IsSkidding && turnAcceleration > 0f
                ? turnAcceleration
                : moveAcceleration * (isDashing ? dashAccelerationMultiplier : 1f);

            float maxSpeedForState = state.IsOnGround && maxGroundSpeed > 0f
                ? maxGroundSpeed
                : maxSpeed;
            float allowedSpeed = maxSpeedForState * (isDashing ? dashSpeedMultiplier : 1f);
            if (speed * sign < allowedSpeed)
            {
                speed = sign * Math.Min(allowedSpeed, speed * sign + acceleration * deltaTime);
            }
        }

        state.Velocity = new Microsoft.Xna.Framework.Vector2(speed, state.Velocity.Y);
        isDashing = false;
        shouldStop = false;
    }
}
