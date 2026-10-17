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
    private const int Width = 32;
    private const int Height = 48;

    private ISprite sprite;
    private Vector2 position;
    private float velocityY;
    private bool isOnGround;
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

    public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, Width, Height);
    public bool IsDefeated => isStomped;
    public bool CanHurtPlayer => !isStomped;

    public event Action<Vector2, Direction> SummonHammer;

    private HammerBroState state = HammerBroState.Idle;
    private float throwElapsed;
    private const float HammerThrowDuration = 0.4f;

    public HammerBro(ISprite sprite, Vector2 initialPosition)
    {
        this.sprite = sprite;
        position = initialPosition;
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


        sprite.Draw(spriteBatch, position, effects: effects);
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
            }

            if (Random.Shared.NextDouble() <
                0.05) // 5% chance to switch moving direction each frame without changin the facing direction
            {
                velocityX = -velocityX;
            }

            position.X += velocityX * deltaTime;
        }

        // LandOn sets this back to true when standing on a block
        isOnGround = false;
        velocityY += Gravity * deltaTime;
        position.Y += velocityY * deltaTime;


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

    public void LandOn(float groundTop)
    {
        // A stomped Hammer Bro falls through the floor and off the screen
        if (isStomped)
        {
            return;
        }

        position.Y = groundTop - Height;
        velocityY = 0f;
        isOnGround = true;
    }

    public void BounceOffWall(float newX)
    {
        position.X = newX;
        velocityX = -velocityX;
    }

    public void ThrowHammer()
    {
        SummonHammer?.Invoke(position, facingDirection);
    }
}
