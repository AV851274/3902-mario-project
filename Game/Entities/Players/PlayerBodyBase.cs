using Mario.Game.Physics;
using System;
using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Mario.Game.Interfaces.Player;
using Microsoft.Xna.Framework;

namespace Mario.Game.Entities.Players;

public abstract class PlayerBodyBase : IPhysicsBody, IPlayerBodyAction
{
    public Vector2 Position { get; set; }

    public Vector2 Velocity { get; set; }

    public bool IsOnGround { get; set; }

    public Vector2 PendingAcceleration { get; set; }

    public virtual float GravityScale => 1f;
    public virtual float MaxFallSpeed => float.PositiveInfinity;

    public ICollider.BlockingModes BlockingMode => ICollider.BlockingModes.None;
    public Box LocalBounds { get; set; } = new Box(0, 0, 36, 48);

    public Box Bounds => new Box(
        Position.X + LocalBounds.X,
        Position.Y + LocalBounds.Y,
        LocalBounds.Width,
        LocalBounds.Height);

    public Direction FacingDirection { get; protected set; } = Direction.Right;

    protected float MoveDirection { get; set; }
    protected bool DashRequested { get; set; }
    protected bool JumpRequested { get; set; }
    protected bool CrouchRequested { get; set; }
    public bool IsCrouching { get; set; }
    public bool IsSkidding { get; set; }

    protected PlayerBodyBase() { }

    protected PlayerBodyBase(IPhysicsBody body)
    {
        Position = body.Position;
        Velocity = body.Velocity;
        IsOnGround = body.IsOnGround;
        PendingAcceleration = body.PendingAcceleration;
        Box bounds = body.Bounds;
        LocalBounds = new Box(
            bounds.X - body.Position.X,
            bounds.Y - body.Position.Y,
            bounds.Width,
            bounds.Height);
        if (body is PlayerBodyBase playerBody)
        {
            FacingDirection = playerBody.FacingDirection;
        }
    }

    // Compute the acceleration needed for the capped next speed, without integrating it.
    protected void PrepareHorizontalMovement(
        float deltaTime, float moveAcceleration, float stopAcceleration, float maxSpeed,
        float turnAcceleration = 0f)
    {
        if (MoveDirection != 0f)
            FacingDirection = MoveDirection < 0f ? Direction.Left : Direction.Right;
        IsSkidding = false;
        if (deltaTime <= 0f)
            return;

        float speed = Velocity.X;
        float nextSpeed = speed;
        if (IsCrouching || MoveDirection == 0f)
        {
            float change = stopAcceleration * deltaTime;
            nextSpeed = speed > 0f ? Math.Max(0f, speed - change) : Math.Min(0f, speed + change);
        }
        else
        {
            IsSkidding = IsOnGround && speed * MoveDirection < 0f;
            float acceleration = IsSkidding && turnAcceleration > 0f ? turnAcceleration : moveAcceleration;
            if (speed * MoveDirection < maxSpeed)
                nextSpeed = MoveDirection * Math.Min(maxSpeed, speed * MoveDirection + acceleration * deltaTime);
        }

        PendingAcceleration += new Vector2((nextSpeed - speed) / deltaTime, 0f);
    }

    protected void ClearInput()
    {
        MoveDirection = 0f;
        DashRequested = false;
        JumpRequested = false;
        CrouchRequested = false;
    }

    public abstract void Move(Direction direction);
    public abstract void Stop();
    public abstract void Jump();
    public abstract void Dash(Direction direction);
    public abstract void Crouch();
    public abstract void Update(GameTime gameTime);
}
