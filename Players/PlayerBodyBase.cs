using Mario.Animation;
using Mario.Interfaces;
using Mario.Interfaces.Player;
using Microsoft.Xna.Framework;

namespace Mario.Players;

public abstract class PlayerBodyBase : IPhysicsBody, IPlayerBodyAction
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public bool IsOnGround { get; set; }
    public bool HasGravity => true;
    public Direction FacingDirection { get; protected set; } = Direction.Right;

    protected PlayerBodyBase()
    {
    }

    protected PlayerBodyBase(IPhysicsBody body)
    {
        Position = body.Position;
        Velocity = body.Velocity;
        IsOnGround = body.IsOnGround;
    }

    public abstract void Move(Direction direction);
    public abstract void Stop();
    public abstract void Jump();
    public abstract void Dash(Direction direction);
    public abstract void Crouch();
    public abstract void Update(GameTime gameTime);
}