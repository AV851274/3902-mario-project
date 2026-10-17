using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Interfaces;
using Mario.Animation;
using Mario.Physics;
using Mario.Physics.Aspects;
using Mario.Physics.State;
using System;

namespace Mario.Entities.Enemies;

public enum HammerBroState
{
    Idle,
    Thrown,
    Stuck
}

public class HammerBro : IEnemy
{
    private const float WalkSpeed = 30f;
    private const float JumpSpeed = -500f;
    private const float Gravity = 1000f;
    private const int Width = 32;
    private const int Height = 48;

    private ISprite sprite;
    private PhysicsState physicsState;
    private readonly PhysicsController physicsController;
    private readonly HorizontalVelocityPhysics horizontalMovement;
    private bool isStomped;

    private Direction facingDirection = Direction.Left;

    public Direction FacingDirection
    {
        get { return facingDirection; }
    }

    public Vector2 Position
    {
        get { return physicsState.Position; }
    }

    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
    public bool IsDefeated => isStomped;
    public bool CanHurtPlayer => !isStomped;

    public event Action<Vector2, Direction> SummonHammer;

    private HammerBroState state = HammerBroState.Idle;
    private float throwElapsed;
    private const float HammerThrowDuration = 0.4f;

    public HammerBro(ISprite sprite, Vector2 initialPosition)
    {
        this.sprite = sprite;
        physicsState = new PhysicsState(initialPosition, Vector2.Zero, isOnGround: true);
        horizontalMovement = new HorizontalVelocityPhysics(-WalkSpeed);
        physicsController = new PhysicsController
        {
            HorizontalVelocityPhysics = horizontalMovement,
            GravityPhysics = new GravityPhysics(Gravity),
            MovementPhysics = new MovementPhysics()
        };
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = facingDirection == Direction.Right
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;

        if (isStomped)
        {
            effects = SpriteEffects.FlipVertically;
        }

        sprite.Draw(spriteBatch, Position, effects: effects);
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (!isStomped)
        {
            if (Random.Shared.NextDouble() < 0.01) // 1% chance to throw a hammer each frame
            {
                state = HammerBroState.Thrown;
                throwElapsed = 0;
                sprite.Play("HammerThrow");
                ThrowHammer();
            }

            if (physicsState.IsOnGround && Random.Shared.NextDouble() < 0.02) // 2% chance to jump each frame
            {
                physicsState.Velocity = new Vector2(physicsState.Velocity.X, JumpSpeed);
                physicsState.IsOnGround = false;
            }

            if (Random.Shared.NextDouble() <
                0.05) // 5% chance to switch moving direction each frame without changin the facing direction
            {
                horizontalMovement.VelocityX = -horizontalMovement.VelocityX;
            }

        }
        else
        {
            horizontalMovement.VelocityX = 0f;
        }

        physicsController.Update(ref physicsState, deltaTime);

        if (state == HammerBroState.Thrown && throwElapsed >= HammerThrowDuration)
        {
            state = HammerBroState.Idle;
            throwElapsed = 0;
            sprite.Play("Idle");
        }
        else
        {
            throwElapsed += deltaTime;
        }

        sprite.UpdateAnimation(gameTime);
    }

    public void Stomp()
    {
        if (isStomped)
            return;

        sprite.Play("Stomped");
        isStomped = true;
        horizontalMovement.VelocityX = 0f;
    }

    public void LandOn(float groundTop)
    {
        // A stomped Hammer Bro falls through the floor and off the screen
        if (isStomped)
        {
            return;
        }

        physicsState.Position = new Vector2(physicsState.Position.X, groundTop - Height);
        physicsState.Velocity = new Vector2(physicsState.Velocity.X, 0f);
        physicsState.IsOnGround = true;
    }

    public void BounceOffWall(float newX)
    {
        physicsState.Position = new Vector2(newX, physicsState.Position.Y);
        horizontalMovement.VelocityX = -horizontalMovement.VelocityX;
    }

    public void ThrowHammer()
    {
        SummonHammer?.Invoke(physicsState.Position, facingDirection);
    }
}
