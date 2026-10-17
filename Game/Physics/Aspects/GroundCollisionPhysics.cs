using Mario.Game.Physics.Interfaces;
using Mario.Game.Physics.State;

namespace Mario.Game.Physics.Aspects;

public sealed class GroundCollisionPhysics : IPhysicsAspect
{
    private readonly float groundY;

    public GroundCollisionPhysics(float groundY)
    {
        this.groundY = groundY;
    }

    public void Apply(ref PhysicsState state, float deltaTime)
    {
        if (state.Position.Y >= groundY)
        {
            state.Position = new Microsoft.Xna.Framework.Vector2(state.Position.X, groundY);
            state.Velocity = new Microsoft.Xna.Framework.Vector2(state.Velocity.X, 0f);
            state.IsOnGround = true;
        }
        else
        {
            state.IsOnGround = false;
        }
    }
}
