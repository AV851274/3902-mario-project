using Mario.Physics.Interfaces;
using Mario.Physics.State;

namespace Mario.Physics.Aspects;

public sealed class HorizontalVelocityPhysics : IPhysicsAspect
{
    public float VelocityX { get; set; }

    public HorizontalVelocityPhysics(float velocityX)
    {
        VelocityX = velocityX;
    }

    public void Apply(ref PhysicsState state, float deltaTime)
    {
        state.Velocity = new Microsoft.Xna.Framework.Vector2(VelocityX, state.Velocity.Y);
    }
}
