using Mario.Physics.Interfaces;
using Mario.Physics.State;

namespace Mario.Physics.Aspects;

public sealed class MovementPhysics : IPhysicsAspect
{
    public void Apply(ref PhysicsState state, float deltaTime)
    {
        state.Position += state.Velocity * deltaTime;
    }
}
