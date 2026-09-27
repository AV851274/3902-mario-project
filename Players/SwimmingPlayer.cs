using System;
using Microsoft.Xna.Framework;
using Game2D.Animation;

public class SwimmingPlayer : IPlayer
{
    private SpriteFactory spriteFactory;
    private Vector2 position;
    private Vector2 velocity;
    private const float MoveAcceleration = 1.5f;
    private const float SwimMaxSpeed = 100f;
    private const float WalkMaxSpeed = 50f;
    private float MoveMaxSpeed = 200f;
    private const float DashMaxSpeed = 200f;
    private const float JumpSpeed = 75f;
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
        SummonFireball?.Invoke(position, facingDirection);
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
            MoveMaxSpeed = WalkMaxSpeed;
        }
    }
}