using System;
using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Mario.Game.Physics;
using Mario.Game.Physics.Aspects;
using Microsoft.Xna.Framework;

namespace Mario.Game.Entities.Players;

public class PlayerBody : PlayerBodyBase
{
    private bool dashHeld;
    private bool crouchHeld;

    private Direction? pendingDirection;
    private readonly HorizontalMovementPhysics horizontalMovement;
    public bool IsSkidding { get; set; }
    public bool IsCrouching { get; private set; }

    public PlayerBody()
        : this(new HorizontalMovementPhysics(
            moveAcceleration: 300f,
            stopAcceleration: 600f,
            maxSpeed: 200f,
            turnAcceleration: 900f,
            dashSpeedMultiplier: 3f,
            dashAccelerationMultiplier: 2f))
    {
    }

    private PlayerBody(HorizontalMovementPhysics horizontalMovement)
        : base(new PhysicsController
        {
            HorizontalMovementPhysics = horizontalMovement,
            JumpingPhysics = new JumpingPhysics(jumpSpeed: 550f),
            GravityPhysics = new GravityPhysics(gravity: 1000f),
            MovementPhysics = new MovementPhysics()
        })
    {
        this.horizontalMovement = horizontalMovement;
    }

    public PlayerBody(IPhysicsBody body)
        : this(body, new HorizontalMovementPhysics(
            moveAcceleration: 300f,
            stopAcceleration: 600f,
            maxSpeed: 200f,
            turnAcceleration: 900f,
            dashSpeedMultiplier: 3f,
            dashAccelerationMultiplier: 2f))
    {
    }

    private PlayerBody(IPhysicsBody body, HorizontalMovementPhysics horizontalMovement)
        : base(body, new PhysicsController
        {
            HorizontalMovementPhysics = horizontalMovement,
            JumpingPhysics = new JumpingPhysics(jumpSpeed: 550f),
            GravityPhysics = new GravityPhysics(gravity: 1000f),
            MovementPhysics = new MovementPhysics()
        })
    {
        this.horizontalMovement = horizontalMovement;
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
        if (pendingDirection.HasValue)
            FacingDirection = pendingDirection.Value;

        float direction = pendingDirection.HasValue
            ? pendingDirection.Value == Direction.Left ? -1f : 1f
            : 0f;
        horizontalMovement.SetInput(direction, dashHeld, IsCrouching);
        UpdatePhysics(deltaTime);
        IsSkidding = horizontalMovement.IsSkidding;

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
        if (!IsOnGround || !RequestJump())
            return;

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
