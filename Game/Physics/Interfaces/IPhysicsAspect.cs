using Mario.Physics.State;

namespace Mario.Physics.Interfaces;

public interface IPhysicsAspect
{
    void Apply(ref PhysicsState state, float deltaTime);
}
