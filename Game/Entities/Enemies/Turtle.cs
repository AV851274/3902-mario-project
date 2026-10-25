using System;
using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public class Turtle : EnemyBase
{
    private const float WalkSpeed = 60f;
    private const float KickSpeed = 6 * WalkSpeed;
    private const float KickGraceTime = 0.25f;
    private const int Width = 40;
    private const int Height = 60;

    // private float leftBound;
    // private float rightBound;
    private bool isStomped;
    private bool isKicked;
    private float kickGraceTimer;
    private Direction facingDirection = Direction.Left;

    public Direction FacingDirection => facingDirection;

    // Walking Koopas and moving shells hurt; a resting shell (or one just kicked) doesn't
    public override bool CanHurtPlayer => !IsDefeated && (!isStomped || (isKicked && kickGraceTimer <= 0f));

    public Turtle(ISprite sprite, Vector2 initialPosition)
        : base(sprite, initialPosition, Width, Height)
    {
        Velocity = new Vector2(-WalkSpeed, 0f);
        // leftBound = initialPosition.X - patrolDistance;
        // rightBound = initialPosition.X + patrolDistance;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = facingDirection == Direction.Right
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;

        if (DefeatedByFireball)
            effects |= SpriteEffects.FlipVertically;

        sprite.Draw(spriteBatch, Position, effects: effects);
    }

    public override void Update(GameTime gameTime)
    {
        if (DefeatedByFireball)
        {
            sprite.UpdateAnimation(gameTime);
            return;
        }

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        kickGraceTimer = Math.Max(0f, kickGraceTimer - deltaTime);

        if (isStomped && !isKicked)
            Velocity = new Vector2(0f, Velocity.Y);
        else if (isKicked && Velocity.X == 0f)
            Velocity =
                new Vector2(KickSpeed * (facingDirection == Direction.Left ? -1 : 1), Velocity.Y);

        sprite.UpdateAnimation(gameTime);
    }

    public override void Stomp()
    {
        if (DefeatedByFireball)
            return;

        if (!isStomped)
        {
            isStomped = true;
            sprite.Play("Stomped");
        }
        else if (!isKicked)
        {
            isKicked = true;
            kickGraceTimer = KickGraceTime;
            Velocity = new Vector2(KickSpeed *
                                   (facingDirection == Direction.Left ? -1 : 1), Velocity.Y);
        }
        else
        {
            isKicked = false;
            Velocity = new Vector2(0f, Velocity.Y);
        }
    }

    protected override void OnWallBounce(float RelativeVelocityBeforeX)
    {
        //-collision.RelativeVelocityBefore.X
        Velocity = new Vector2(-RelativeVelocityBeforeX, Velocity.Y);
        facingDirection = Velocity.X < 0f ? Direction.Left : Direction.Right;
    }
}