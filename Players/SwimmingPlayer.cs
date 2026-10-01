using System;
using Microsoft.Xna.Framework;
using Game2D.Animation;
using Game2D.Interfaces;
using Microsoft.Xna.Framework.Graphics;

public class SwimmingPlayer : IPlayer
{
    private IMarioState powerState;
    private SpriteFactory spriteFactory;
    private Vector2 position;
    private Vector2 velocity;
    private const float MoveAcceleration = 1.5f;
    private const float SwimMaxSpeed = 100f;
    private const float FallMaxSpeed = 100f;
    private const float WalkMaxSpeed = 50f;
    private float MoveMaxSpeed = 200f;
    private const float DashMaxSpeed = 200f;
    private const float JumpSpeed = 75f;
    private float jumpTimer = 0.61f;
    private const float Gravity = 200f;
    private const float GroundY = 400f;
    private bool isOnGround;
    private Direction facingDirection = Direction.Right;
    public event Action<Vector2, Direction> SummonFireball;
    public Vector2 Position => position;
    public bool IsMoving => velocity.X != 0;
    public Direction FacingDirection => facingDirection;
    public bool IsOnGround => isOnGround;

    public SwimmingPlayer(SpriteFactory spriteFactory, Vector2 startingPosition)
    {
        this.spriteFactory = spriteFactory;
        powerState = new SwimSmallMarioState(spriteFactory);
        position = startingPosition;
        velocity = Vector2.Zero;
        isOnGround = false;
    }

    public void MoveLeft()
    {
        if (velocity.X > -MoveMaxSpeed)
        {
            velocity.X -= MoveAcceleration;
        }

        facingDirection = Direction.Left;
    }

    public void MoveRight()
    {
        if (velocity.X < MoveMaxSpeed)
        {
            velocity.X += MoveAcceleration;
        }

        facingDirection = Direction.Right;
    }

    public void StopMoving() 
    {
        if (velocity.X < -MoveAcceleration)
        {
            velocity.X += MoveAcceleration;
        }
        else if (velocity.X > MoveAcceleration)
        {
            velocity.X -= MoveAcceleration;
        }
        else
        {
            velocity.X = 0;
        }
    }

    public void Jump()
    {
        velocity.Y = -JumpSpeed;
        isOnGround = false;
        MoveMaxSpeed = SwimMaxSpeed;
        jumpTimer = 0f;
    }

    public void Crouch()
    {
        // Can't crouch while swimming
    }

    public void Dash()
    {
        velocity.X = 600f * (facingDirection == Direction.Right ? 1f : -1f);
    }

    public void DashLeft()
    {
        this.MoveLeft();
    }

    public void DashRight()
    {
        this.MoveRight();
    }

    public void Attack()
    {
        if (powerState.CanShootFireball)
        {
            SummonFireball?.Invoke(position, facingDirection);
        }
    }

    public void BecomeSmall()
    {
        powerState = new SwimSmallMarioState(spriteFactory);
    }

    public void BecomeBig()
    {
        powerState = new SwimBigMarioState(spriteFactory);
    }

    public void BecomeFire()
    {
        powerState = new SwimFireMarioState(spriteFactory);
    }

    public void TakeDamage()
    {
        if (powerState.marioPower == MarioPower.Fire)
        {
            BecomeBig();
        }
        else if (powerState.marioPower == MarioPower.Big)
        {
            BecomeSmall();
        }
        else if (powerState.marioPower == MarioPower.Small)
        {
            //Die
        }
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime =
            (float)gameTime.ElapsedGameTime.TotalSeconds;

        isOnGround = GravityPhysics.Apply(
            ref position,
            ref velocity,
            deltaTime,
            Gravity,
            GroundY,
            FallMaxSpeed);

        jumpTimer += deltaTime;

        if (isOnGround)
        {
            MoveMaxSpeed = WalkMaxSpeed;
        }

        if (jumpTimer < 0.4f)
        {
            powerState.Sprite.Play("Swim");
        }
        else if (!isOnGround)
        {
            powerState.Sprite.Play("Float");
        }
        else if (IsMoving)
        {
            powerState.Sprite.Play("Walk");
        }
        else
        {
            powerState.Sprite.Play("Stand");
        }

        powerState.Sprite.UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = facingDirection == Direction.Left
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;

        powerState.Sprite.Draw(spriteBatch, position + powerState.DrawOffset, effects: effects);
    }
}