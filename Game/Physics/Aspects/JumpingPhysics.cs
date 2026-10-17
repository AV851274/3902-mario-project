using System;
using Mario.Game.Physics.Interfaces;
using Mario.Game.Physics.State;
using Microsoft.Xna.Framework;

namespace Mario.Game.Physics.Aspects;

public sealed class JumpingPhysics : IPhysicsAspect
{
    private readonly float jumpSpeed;
    private bool jumpRequested;

    public JumpingPhysics(float jumpSpeed)
    {
        if (jumpSpeed <= 0f)
            throw new ArgumentOutOfRangeException(nameof(jumpSpeed));

        this.jumpSpeed = jumpSpeed;
    }

    public void RequestJump()
    {
        jumpRequested = true;
    }

    public void Apply(ref PhysicsState state, float deltaTime)
    {
        if (jumpRequested && state.IsOnGround)
        {
            state.Velocity = new Vector2(state.Velocity.X, -jumpSpeed);
            state.IsOnGround = false;
        }

        jumpRequested = false;
    }
}
