using System;
using Mario.Animation;
using Mario.Interfaces;
using Microsoft.Xna.Framework;

namespace Mario.Players;

public class SwimmingPlayerBody : PlayerBodyBase
{
    //--------consts-------------------------------
    private const float MoveAcceleration = 90f;
    private const float StopAcceleration = 90f;
    private const float SwimMaxSpeed = 100f;
    private const float WalkMaxSpeed = 50f;
    private const float JumpSpeed = 75f;
    private const float StrokeAnimationTime = 0.4f;
    // ---------------------------------------------

    private Direction? pendingDirection;
    private float timeSinceStroke = float.PositiveInfinity;

    public bool IsSwimming => !IsOnGround && timeSinceStroke < StrokeAnimationTime;

    public SwimmingPlayerBody()
    {
    }

    public SwimmingPlayerBody(IPhysicsBody body) : base(body)
    {
    }

    public override void Move(Direction direction)
    {
        pendingDirection = direction;
    }

    public override void Stop()
    {
        pendingDirection = null;
    }

    public override void Jump()
    {
        Velocity = new Vector2(Velocity.X, -JumpSpeed);
        IsOnGround = false;
        timeSinceStroke = 0f;
    }

    public override void Dash(Direction direction)
    {
        Move(direction);
    }


    public override void Crouch()
    {
        // Swimming does not support crouching.
    }

    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        timeSinceStroke += deltaTime;

        float speed = Velocity.X;
        if (pendingDirection is null)
        {
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
            float maxSpeed = IsOnGround ? WalkMaxSpeed : SwimMaxSpeed;

            if (speed * sign < maxSpeed)
                speed = sign * Math.Min(maxSpeed, speed * sign + MoveAcceleration * deltaTime);
        }

        Velocity = new Vector2(speed, Velocity.Y);
        pendingDirection = null;
    }
}