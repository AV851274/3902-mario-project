using Microsoft.Xna.Framework;
using Game2D.Animation;

public class Player : IPlayer
{
    private Vector2 position;
    private Vector2 velocity;
    private const float MoveAcceleration = 5;
    private const float MoveMaxSpeed = 200f;
    private const float DashMaxSpeed = 600f;
    private const float JumpSpeed = 450f;
    private const float Gravity = 1000f;
    private const float GroundY = 400f;
    private bool isOnGround;
    private Direction facingDirection = Direction.Right;

    public Vector2 Position
    {
        get
        {
            return position;
        }
    }

    public bool IsMoving
    {
        get
        {
            return velocity.X != 0;
        }
    }

    public Direction FacingDirection
    {
        get
        {
            return facingDirection;
        }
    }
    public bool IsOnGround
    {
        get
        {
            return isOnGround;
        }
    }

    public Player(Vector2 startingPosition)
    {
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

    public void StopMoving() //Update to resolve issues with movement
    {
        if (velocity.X < -MoveAcceleration)
        {
            velocity.X += MoveAcceleration;
        }
        else if (velocity.X > MoveAcceleration)
        {
            velocity.X -= MoveAcceleration;
        } else
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
        if (velocity.X > -3f*MoveMaxSpeed)
        {
            velocity.X -= 2f*MoveAcceleration;
        }
        facingDirection = Direction.Left;
    }

    public void DashRight()
    {
        if (velocity.X < 3f*MoveMaxSpeed)
        {
            velocity.X += 2f*MoveAcceleration;
        }
        facingDirection = Direction.Right;
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
    }
}