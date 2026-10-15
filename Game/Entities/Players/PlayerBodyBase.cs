using Mario.Animation;
using Mario.Interfaces;
using Mario.Interfaces.Player;
using Mario.Physics;
using Mario.Physics.State;
using Microsoft.Xna.Framework;

namespace Mario.Players;

public abstract class PlayerBodyBase : IPhysicsBody, IPlayerBodyAction
{
    private readonly PhysicsController physicsController;
    private PhysicsState physicsState;

    public Vector2 Position
    {
        get => physicsState.Position;
        set => physicsState.Position = value;
    }

    public Vector2 Velocity
    {
        get => physicsState.Velocity;
        set => physicsState.Velocity = value;
    }

    public bool IsOnGround
    {
        get => physicsState.IsOnGround;
        set => physicsState.IsOnGround = value;
    }

    public Direction FacingDirection { get; protected set; } = Direction.Right;

    protected PlayerBodyBase(PhysicsController physicsController)
    {
        this.physicsController = physicsController;
    }

    protected PlayerBodyBase(IPhysicsBody body, PhysicsController physicsController): this(physicsController)
    {
        physicsState = new PhysicsState(body.Position, body.Velocity, body.IsOnGround);
    }

    protected void UpdatePhysics(float deltaTime)
    {
        physicsController.Update(ref physicsState, deltaTime);
    }

    protected bool RequestJump()
    {
        return physicsController.RequestJump();
    }

    public abstract void Move(Direction direction);
    public abstract void Stop();
    public abstract void Jump();
    public abstract void Dash(Direction direction);
    public abstract void Crouch();
    public abstract void Update(GameTime gameTime);
}