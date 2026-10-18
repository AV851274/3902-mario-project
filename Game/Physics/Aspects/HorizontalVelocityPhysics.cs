using Mario.Game.Physics.Interfaces;
using Mario.Game.Physics.State;

namespace Mario.Game.Physics.Aspects;

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
