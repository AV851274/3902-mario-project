using Microsoft.Xna.Framework;

public class Player : IPlayer
{
    private Vector2 position;
    private Vector2 velocity;
    private const float MoveSpeed = 200f;
    private const float JumpSpeed = 450f;
    private const float Gravity = 1000f;
    private const float GroundY = 400f;
    private bool isOnGround;
    private int facingDirection = 1;

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

    public int FacingDirection
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
        velocity.X = -MoveSpeed;
        facingDirection = -1;
    }

    public void MoveRight()
    {
        velocity.X = MoveSpeed;
        facingDirection = 1;
    }

    public void StopMoving()
    {
        velocity.X = 0;
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
        velocity.X = 600f * facingDirection;
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