using System;
using Mario.Animation;
using Mario.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Enemies;

public class Turtle : IEnemy
{
    private const float WalkSpeed = 60f;
    private const float KickSpeed = 6 * WalkSpeed;
    private const float Gravity = 1000f;
    private const float KickGraceTime = 0.25f;
    private const int Width = 32;
    private const int Height = 48;

    private ISprite sprite;
    private Vector2 position;
    private float velocityX = -WalkSpeed;
    private float velocityY;
    private bool isStomped;
    private bool isKicked;
    private float kickGraceTimer;
    private Direction facingDirection = Direction.Left;

    public Direction FacingDirection => facingDirection;
    public Vector2 Position => position;
    public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, Width, Height);

    // A shell stays in the level, so a Koopa is never fully defeated
    public bool IsDefeated => false;

    // Walking Koopas and moving shells hurt; a resting shell (or one just kicked) doesn't
    public bool CanHurtPlayer => !isStomped || (isKicked && kickGraceTimer <= 0f);

    public Turtle(ISprite sprite, Vector2 initialPosition)
    {
        this.sprite = sprite;
        position = initialPosition;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = facingDirection == Direction.Right
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;

        sprite.Draw(spriteBatch, position, effects: effects);
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        kickGraceTimer = Math.Max(0f, kickGraceTimer - deltaTime);

        velocityY += Gravity * deltaTime;
        position.Y += velocityY * deltaTime;

        if (!isStomped || isKicked)
        {
            position.X += velocityX * deltaTime;
        }

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
            velocityX = KickSpeed * (facingDirection == Direction.Left ? -1 : 1);
        }
        else
        {
            isKicked = false;
        }
    }

    public void LandOn(float groundTop)
    {
        position.Y = groundTop - Height;
        velocityY = 0f;
    }

    public void BounceOffWall(float newX)
    {
        position.X = newX;
        velocityX = -velocityX;
        facingDirection = velocityX < 0 ? Direction.Left : Direction.Right;
    }
}
