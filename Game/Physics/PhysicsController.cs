using Mario.Physics.Aspects;
using Mario.Physics.State;

namespace Mario.Physics;

public sealed class PhysicsController
{
    public HorizontalMovementPhysics HorizontalMovementPhysics { get; init; }
    public HorizontalVelocityPhysics HorizontalVelocityPhysics { get; init; }
    public JumpingPhysics JumpingPhysics { get; init; }
    public GravityPhysics GravityPhysics { get; init; }
    public MovementPhysics MovementPhysics { get; init; }
    public GroundCollisionPhysics GroundCollisionPhysics { get; init; }
    public BouncingPhysics BouncingPhysics { get; init; }

    public bool RequestJump()
    {
        if (JumpingPhysics is null)
            return false;

        JumpingPhysics.RequestJump();
        return true;
    }

    public void Update(ref PhysicsState state, float deltaTime)
    {
        JumpingPhysics?.Apply(ref state, deltaTime);
        HorizontalMovementPhysics?.Apply(ref state, deltaTime);
        HorizontalVelocityPhysics?.Apply(ref state, deltaTime);
        GravityPhysics?.Apply(ref state, deltaTime);
        MovementPhysics?.Apply(ref state, deltaTime);
        GroundCollisionPhysics?.Apply(ref state, deltaTime);
        BouncingPhysics?.Apply(ref state, deltaTime);
    }
}
