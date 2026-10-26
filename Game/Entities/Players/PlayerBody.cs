using System;
using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;

namespace Mario.Game.Entities.Players;

public class PlayerBody : PlayerBodyBase
{
    private const float MoveAcceleration = 300f;
    private const float StopAcceleration = 600f;
    private const float MaxSpeed = 200f;
    private const float TurnAcceleration = 900f;
    private const float DashSpeedMultiplier = 3f;
    private const float DashAccelerationMultiplier = 2f;
    private float JumpSpeed => Math.Abs(Velocity.X) > 190f ? 550f : 500f;
    private bool IsRising => Velocity.Y < -20f;
    public override float GravityScale => IsRising && IsJumpHeld ? 0.62f : IsRising ? 2f : 1f;
    public PlayerBody() { }

    public PlayerBody(IPhysicsBody body) : base(body) { }

    public override void Move(Direction direction)
    {
        MoveDirection = direction == Direction.Left ? -1f : 1f;
        DashRequested = false;
    }

    public override void Stop()
    {
        MoveDirection = 0f;
        DashRequested = false;
    }

    public override void Jump()
    {
        SetJumpHeld(true);
        if (IsOnGround)
            JumpRequested = true;
    }

    public override void Dash(Direction direction)
    {
        Move(direction);
        DashRequested = true;
    }

    public override void Crouch() => CrouchRequested = true;

    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        IsCrouching = CrouchRequested && IsOnGround;
        if (JumpRequested && IsOnGround)
        {
            Velocity = new Vector2(Velocity.X, -JumpSpeed);
            IsOnGround = false;
            IsCrouching = false;
        }
        PrepareHorizontalMovement(deltaTime,
            MoveAcceleration * (DashRequested ? DashAccelerationMultiplier : 1f),
            StopAcceleration, MaxSpeed * (DashRequested ? DashSpeedMultiplier : 1f),
            TurnAcceleration);
        ClearInput();
    }
}
