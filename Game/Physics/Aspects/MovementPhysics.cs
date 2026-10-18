using Mario.Game.Physics.Interfaces;
using Mario.Game.Physics.State;

namespace Mario.Game.Physics.Aspects;

public sealed class MovementPhysics : IPhysicsAspect
{
    public void Apply(ref PhysicsState state, float deltaTime)
    {
        state.Position += state.Velocity * deltaTime;
    }
}
