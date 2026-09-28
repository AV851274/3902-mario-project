using System;
using Microsoft.Xna.Framework;
using Game2D.Animation;
using Microsoft.Xna.Framework.Graphics;

public class Player : IPlayer
{
    private IMarioState powerState;
    private SpriteFactory spriteFactory;
    private Vector2 position;
    private Vector2 velocity;
    private const float MoveAcceleration = 5f;
    private const float MoveMaxSpeed = 200f;
    private const float JumpSpeed = 450f;
    private const float Gravity = 1000f;
    private const float GroundY = 400f;
    private bool isOnGround;
    private Direction facingDirection = Direction.Right;

    public event Action<Vector2, Direction> SummonFireball;

    public Vector2 Position => position;
    public bool IsMoving => velocity.X != 0;
    public Direction FacingDirection => facingDirection;
    public bool IsOnGround => isOnGround;

    public Player(SpriteFactory spriteFactory, Vector2 startingPosition)
    {
        this.spriteFactory = spriteFactory;
        powerState = new SmallMarioState(spriteFactory);
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
        if (isOnGround)
        {
            velocity.Y = -JumpSpeed;
            isOnGround = false;
        }
    }

    public void Dash()
    {
        velocity.X = 600f * (facingDirection == Direction.Right ? 1f : -1f);
    }

    public void DashLeft()
    {
        if (velocity.X > -3f * MoveMaxSpeed)
        {
            velocity.X -= 2f * MoveAcceleration;
        }

        facingDirection = Direction.Left;
    }

    public void DashRight()
    {
        if (velocity.X < 3f * MoveMaxSpeed)
        {
            velocity.X += 2f * MoveAcceleration;
        }

        facingDirection = Direction.Right;
    }

    public void Attack()
    {
        if (powerState.CanShootFireball)
        {
            SummonFireball?.Invoke(position, facingDirection);
        }
    }

    public void SetPowerState(IMarioState newState)
    {
        powerState = newState;
    }

    public void BecomeSmall()
    {
        powerState = new SmallMarioState(spriteFactory);
    }

    public void BecomeBig()
    {
        powerState = new BigMarioState(spriteFactory);
    }

    public void BecomeFire()
    {
        powerState = new FireMarioState(spriteFactory);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = facingDirection == Direction.Right
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;

        powerState.Sprite.Draw(spriteBatch, position + powerState.DrawOffset, effects: effects);
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime =
            (float)gameTime.ElapsedGameTime.TotalSeconds;

        velocity.Y += Gravity * deltaTime;

        position += velocity * deltaTime;

        if (position.Y >= GroundY)
        {
            position.Y = GroundY;
            velocity.Y = 0;
            isOnGround = true;
        }

        if (!isOnGround)
        {
            powerState.Sprite.Play("Jump");
        }
        else if (IsMoving)
        {
            powerState.Sprite.Play("Run");
        }
        else
        {
            powerState.Sprite.Play("Idle");
        }

        powerState.Sprite.UpdateAnimation(gameTime);
    }

    public void Draw (SpriteBatch spriteBatch)
    {
        //Fill with what Sam did
    }
}