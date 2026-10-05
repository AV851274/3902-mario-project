using Mario.Animation;
using Mario.Interfaces;
using Mario.Physics;
using Mario.Players.PlayerStates;
using Mario.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mario.Players;

public class Player : IPlayer
{
    private IMarioState powerState;
    private SpriteFactory spriteFactory;
    private Vector2 position;
    private Vector2 velocity;
    private const float MoveAcceleration = 5f;
    private const float TurnAcceleration = 15f;
    private const float MoveMaxSpeed = 200f;
    private const float JumpSpeed = 450f;
    private const float Gravity = 1000f;
    private const float GroundY = 400f;
    private const float ThrowAnimationTime = 0.3f;
    private bool isOnGround;
    private bool isSkidding = false;
    private bool crouchHeld = false; // set by the controller each frame S is held
    private bool isCrouching = false;
    private float timeSinceThrow = 0;
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
        if (isCrouching)
        {
            StopMoving(); // can't walk while crouched, just slide to a stop
            return;
        }

        isSkidding = isOnGround && velocity.X > 0;

        if (isSkidding)
        {
            velocity.X -= TurnAcceleration;
        }
        else if (velocity.X > -MoveMaxSpeed)
        {
            velocity.X -= MoveAcceleration;
        }

        facingDirection = Direction.Left;
    }

    public void MoveRight()
    {
        if (isCrouching)
        {
            StopMoving(); // can't walk while crouched, just slide to a stop
            return;
        }

        isSkidding = isOnGround && velocity.X < 0;

        if (isSkidding)
        {
            velocity.X += TurnAcceleration;
        }
        else if (velocity.X < MoveMaxSpeed)
        {
            velocity.X += MoveAcceleration;
        }

        facingDirection = Direction.Right;
    }

    public void StopMoving()
    {
        isSkidding = false;

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

    public void Crouch()
    {
        crouchHeld = true;
    }

    public void Dash()
    {
        velocity.X = 600f * (facingDirection == Direction.Right ? 1f : -1f);
    }

    public void DashLeft()
    {
        if (isCrouching)
        {
            StopMoving(); // can't walk while crouched, just slide to a stop
            return;
        }

        isSkidding = isOnGround && velocity.X > 0;

        if (isSkidding)
        {
            velocity.X -= TurnAcceleration;
        }
        else if (velocity.X > -3f * MoveMaxSpeed)
        {
            velocity.X -= 2f * MoveAcceleration;
        }

        facingDirection = Direction.Left;
    }

    public void DashRight()
    {
        if (isCrouching)
        {
            StopMoving(); // can't walk while crouched, just slide to a stop
            return;
        }

        isSkidding = isOnGround && velocity.X < 0;

        if (isSkidding)
        {
            velocity.X += TurnAcceleration;
        }
        else if (velocity.X < 3f * MoveMaxSpeed)
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
            timeSinceThrow = 0f;
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

        timeSinceThrow += deltaTime;

        isOnGround = GravityPhysics.Apply(ref position, ref velocity, deltaTime, Gravity, GroundY);

        isCrouching = crouchHeld && isOnGround && powerState.marioPower != MarioPower.Small;
        crouchHeld = false; // the controller sets it again next frame if S is still held

        if (!isOnGround)
        {
            powerState.Sprite.Play("Jump");
        }
        else if (timeSinceThrow < ThrowAnimationTime)
        {
            powerState.Sprite.Play("Throw");
        }
        else if (isCrouching)
        {
            powerState.Sprite.Play("Crouch");
        }
        else if (isSkidding)
        {
            powerState.Sprite.Play("Skid");
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
}