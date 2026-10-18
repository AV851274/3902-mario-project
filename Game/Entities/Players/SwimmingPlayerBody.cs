using System;
using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Mario.Game.Physics;
using Mario.Game.Physics.Aspects;
using Microsoft.Xna.Framework;

namespace Mario.Game.Entities.Players;

public class SwimmingPlayerBody : PlayerBodyBase
{
    //--------consts-------------------------------
    private const float JumpSpeed = 75f;
    private const float StrokeAnimationTime = 0.4f;
    // ---------------------------------------------

    private Direction? pendingDirection;
    private readonly HorizontalMovementPhysics horizontalMovement;
    private float timeSinceStroke = float.PositiveInfinity;

    public bool IsSwimming => !IsOnGround && timeSinceStroke < StrokeAnimationTime;

    public SwimmingPlayerBody()
        : this(new HorizontalMovementPhysics(
            moveAcceleration: 90f,
            stopAcceleration: 90f,
            maxSpeed: 100f,
            maxGroundSpeed: 50f))
    {
    }

    private SwimmingPlayerBody(HorizontalMovementPhysics horizontalMovement)
        : base(new PhysicsController
        {
            HorizontalMovementPhysics = horizontalMovement,
            GravityPhysics = new GravityPhysics(gravity: 200f, maxFallSpeed: 100f),
            MovementPhysics = new MovementPhysics()
        })
    {
        this.horizontalMovement = horizontalMovement;
    }

    public SwimmingPlayerBody(IPhysicsBody body)
        : this(body, new HorizontalMovementPhysics(
            moveAcceleration: 90f,
            stopAcceleration: 90f,
            maxSpeed: 100f,
            maxGroundSpeed: 50f))
    {
    }

    private SwimmingPlayerBody(IPhysicsBody body, HorizontalMovementPhysics horizontalMovement)
        : base(body, new PhysicsController
        {
            HorizontalMovementPhysics = horizontalMovement,
            GravityPhysics = new GravityPhysics(gravity: 200f, maxFallSpeed: 100f),
            MovementPhysics = new MovementPhysics()
        })
    {
        this.horizontalMovement = horizontalMovement;
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

        if (pendingDirection.HasValue)
            FacingDirection = pendingDirection.Value;
        float direction = pendingDirection.HasValue
            ? pendingDirection.Value == Direction.Left ? -1f : 1f
            : 0f;
        horizontalMovement.SetInput(direction, isDashing: false, shouldStop: false);
        UpdatePhysics(deltaTime);
        pendingDirection = null;
    }
}