using Mario.Game.Physics.State;

namespace Mario.Game.Physics.Interfaces;

public interface IPhysicsAspect
{
    void Apply(ref PhysicsState state, float deltaTime);
}
