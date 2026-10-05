using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Interfaces;
using Mario.Animation;
using System;

namespace Mario.Enemies;

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

    private ISprite sprite;
    private Vector2 position;
    private float velocityY;
    private float groundY;
    private bool isOnGround = true;
    private float velocityX = -WalkSpeed;
    private bool isStomped;

    private float rotation = 0f;

    private Direction facingDirection = Direction.Left;

    public Direction FacingDirection
    {
        get { return facingDirection; }
    }

    public Vector2 Position
    {
        get { return position; }
    }

    public event Action<Vector2, Direction> SummonHammer;

    private HammerBroState state = HammerBroState.Idle;
    private float throwElapsed;
    private const float HammerThrowDuration = 0.4f;

    public HammerBro(ISprite sprite, Vector2 initialPosition, int patrolDistance = 150)
    {
        this.sprite = sprite;
        position = initialPosition;
        groundY = initialPosition.Y;
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = facingDirection == Direction.Right
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;


        sprite.Draw(spriteBatch, position, rotation, effects: effects);
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

            if (isOnGround && Random.Shared.NextDouble() < 0.02) // 2% chance to jump each frame
            {
                velocityY = JumpSpeed;
                isOnGround = false;
            }

            if (Random.Shared.NextDouble() <
                0.05) // 5% chance to switch moving direction each frame without changin the facing direction
            {
                velocityX = -velocityX;
            }

            velocityY += Gravity * deltaTime;
            position.Y += velocityY * deltaTime;
            if (position.Y >= groundY)
            {
                position.Y = groundY;
                velocityY = 0f;
                isOnGround = true;
            }

            position.X += velocityX * deltaTime;

        } else
        {
            position.Y += velocityY * deltaTime;
            velocityY += Gravity * deltaTime;
        }


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
        rotation = 180f;
        isStomped = true;
    }

    public void ThrowHammer()
    {
        SummonHammer?.Invoke(position, facingDirection);
    }
}