using System;
using Mario.Animation;
using Mario.Interfaces;
using Microsoft.Xna.Framework;

namespace Mario.Players;

public class PlayerBody : PlayerBodyBase
{
    //--------consts-------------------------------
    private const float MoveAcceleration = 300f;
    private const float StopAcceleration = 600f;
    private const float TurnAcceleration = 900f;
    private const float MoveMaxSpeed = 200f;
    private const float JumpSpeed = 450f;
    //---------------------------------------------

    private bool dashHeld;
    private bool crouchHeld;

    private Direction? pendingDirection;
    public bool IsSkidding { get; set; }
    public bool IsCrouching { get; private set; }

    public PlayerBody()
    {
    }

    public PlayerBody(IPhysicsBody body) : base(body)
    {
    }

    public override void Move(Direction direction)
    {
        pendingDirection = direction;
        dashHeld = false;
    }

    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        IsCrouching = crouchHeld && IsOnGround;
        IsSkidding = false;

        float speed = Velocity.X;
        if (IsCrouching || pendingDirection is null)
        {
            // Stop moving
            float change = StopAcceleration * deltaTime;
            if (speed > 0f)
                speed = Math.Max(0f, speed - change);
            else
                speed = Math.Min(0f, speed + change);
        }
        else
        {
            Direction direction = pendingDirection.Value;
            FacingDirection = direction;
            float sign = direction == Direction.Left ? -1f : 1f;
            IsSkidding = IsOnGround && speed * sign < 0f;

            float acceleration;
            if (IsSkidding)
                acceleration = TurnAcceleration;
            else if (dashHeld)
                acceleration = 2f * MoveAcceleration;
            else
                acceleration = MoveAcceleration;

            float maxSpeed = dashHeld ? 3f * MoveMaxSpeed : MoveMaxSpeed;

            if (speed * sign < maxSpeed)
                speed = sign * Math.Min(maxSpeed, speed * sign + acceleration * deltaTime);
        }

        Velocity = new Vector2(speed, Velocity.Y);
        pendingDirection = null;
        dashHeld = false;
        crouchHeld = false;
    }

    public override void Stop()
    {
        pendingDirection = null;
        dashHeld = false;
    }

    public override void Jump()
    {
        if (!IsOnGround)
            return;

        Velocity = new Vector2(Velocity.X, -JumpSpeed);
        IsOnGround = false;
        IsCrouching = false;
        crouchHeld = false;
        IsSkidding = false;
    }

    public override void Dash(Direction direction)
    {
        pendingDirection = direction;
        dashHeld = true;
    }

    public override void Crouch()
    {
        crouchHeld = true;
    }
}
