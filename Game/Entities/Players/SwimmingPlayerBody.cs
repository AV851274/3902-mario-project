using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;

namespace Mario.Game.Entities.Players;

public class SwimmingPlayerBody : PlayerBodyBase
{
    private const float MoveAcceleration = 90f;
    private const float StopAcceleration = 90f;
    private const float MaxSpeed = 100f;
    private const float MaxGroundSpeed = 50f;
    private const float StrokeSpeed = 75f;
    private const float StrokeAnimationTime = 0.4f;
    private float timeSinceStroke = float.PositiveInfinity;

    public override float GravityScale => 0.2f;
    public override float MaxFallSpeed => 100f;
    public bool IsSwimming => !IsOnGround && timeSinceStroke < StrokeAnimationTime;

    public SwimmingPlayerBody()
    {
    }

    public SwimmingPlayerBody(IPhysicsBody body) : base(body)
    {
    }

    public override void Move(Direction direction)
    {
        MoveDirection = direction == Direction.Left ? -1f : 1f;
    }

    public override void Stop() => MoveDirection = 0f;

    public override void Jump()
    {
        JumpRequested = true;
        timeSinceStroke = 0f;
    }

    public override void Dash(Direction direction) => Move(direction);

    public override void Crouch()
    {
    }

    public override void Update(GameTime gameTime)
    {
        timeSinceStroke += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (JumpRequested)
        {
            Velocity = new Vector2(Velocity.X, -StrokeSpeed);
            IsOnGround = false;
        }

        PrepareHorizontalMovement((float)gameTime.ElapsedGameTime.TotalSeconds,
            moveAcceleration: MoveAcceleration, stopAcceleration: StopAcceleration,
            maxSpeed: IsOnGround ? MaxGroundSpeed : MaxSpeed);
        ClearInput();
    }
}