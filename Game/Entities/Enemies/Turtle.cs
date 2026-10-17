using System;
using Mario.Animation;
using Mario.Interfaces;
using Mario.Physics;
using Mario.Physics.Aspects;
using Mario.Physics.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Entities.Enemies;

public class Turtle : IEnemy
{
    private const float WalkSpeed = 60f;
    private const float KickSpeed = 6 * WalkSpeed;
    private const float Gravity = 1000f;
    private const float KickGraceTime = 0.25f;
    private const int Width = 32;
    private const int Height = 48;

    private ISprite sprite;
    private readonly PhysicsController physicsController;
    private readonly HorizontalVelocityPhysics horizontalMovement;
    private readonly GravityPhysics gravityPhysics;
    private PhysicsState physicsState;
    private float leftBound;
    private float rightBound;
    private bool isStomped;
    private bool isKicked;
    private float kickGraceTimer;
    private Direction facingDirection = Direction.Left;

    public Direction FacingDirection => facingDirection;
    public Vector2 Position => physicsState.Position;
    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

    // A shell stays in the level, so a Koopa is never fully defeated
    public bool IsDefeated => false;

    // Walking Koopas and moving shells hurt; a resting shell (or one just kicked) doesn't
    public bool CanHurtPlayer => !isStomped || (isKicked && kickGraceTimer <= 0f);

    public Turtle(ISprite sprite, Vector2 initialPosition, int patrolDistance = 150)
    {
        this.sprite = sprite;
        physicsState = new PhysicsState(initialPosition, Vector2.Zero);
        horizontalMovement = new HorizontalVelocityPhysics(-WalkSpeed);
        gravityPhysics = new GravityPhysics(Gravity);
        physicsController = new PhysicsController
        {
            HorizontalVelocityPhysics = horizontalMovement,
            GravityPhysics = gravityPhysics,
            MovementPhysics = new MovementPhysics()
        };
        leftBound = initialPosition.X - patrolDistance;
        rightBound = initialPosition.X + patrolDistance;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = facingDirection == Direction.Right
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;

        sprite.Draw(spriteBatch, Position, effects: effects);
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        kickGraceTimer = Math.Max(0f, kickGraceTimer - deltaTime);

        if (!isStomped || isKicked)
        {
            //TURN TURTLE AROUND WHEN IT HITS THE EDGE OF ITS PATROL AREA
            if (Position.X <= leftBound || Position.X >= rightBound)
            {
                horizontalMovement.VelocityX = -horizontalMovement.VelocityX;
                facingDirection = horizontalMovement.VelocityX < 0
                    ? Direction.Left
                    : Direction.Right;
            }
        }

        if (isStomped && !isKicked)
            horizontalMovement.VelocityX = 0f;
        else if (isKicked && horizontalMovement.VelocityX == 0f)
            horizontalMovement.VelocityX = KickSpeed *
                (facingDirection == Direction.Left ? -1 : 1);

        physicsController.Update(ref physicsState, (float)gameTime.ElapsedGameTime.TotalSeconds);
        sprite.UpdateAnimation(gameTime);
    }

    public void Stomp()
    {
        if (!isStomped)
        {
            isStomped = true;
            sprite.Play("Stomped");
        }
        else if (!isKicked)
        {
            isKicked = true;
            kickGraceTimer = KickGraceTime;
            horizontalMovement.VelocityX = KickSpeed *
                (facingDirection == Direction.Left ? -1 : 1);
        }
        else
        {
            isKicked = false;
            horizontalMovement.VelocityX = 0f;
        }
    }

    public void LandOn(float groundTop)
    {
        physicsState.Position = new Vector2(physicsState.Position.X, groundTop - Height);
        physicsState.Velocity = new Vector2(physicsState.Velocity.X, 0f);
        physicsState.IsOnGround = true;
    }

    public void BounceOffWall(float newX)
    {
        physicsState.Position = new Vector2(newX, physicsState.Position.Y);
        horizontalMovement.VelocityX = -horizontalMovement.VelocityX;
        facingDirection = horizontalMovement.VelocityX < 0 ? Direction.Left : Direction.Right;
    }
}
