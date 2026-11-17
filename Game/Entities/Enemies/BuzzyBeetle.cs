using System;
using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public class BuzzyBeetle : EnemyBase
{
    private const float KickSpeed =
        GameConstants.Enemies.KickSpeedMultiplier * GameConstants.Enemies.StandardWalkSpeed;

    // private float leftBound;
    // private float rightBound;
    private bool isStomped;
    private bool isKicked;
    private float kickGraceTimer;
    private Direction facingDirection = Direction.Left;

    public Direction FacingDirection => facingDirection;

    // Walking Koopas and moving shells hurt; a resting shell (or one just kicked) doesn't
    public override bool CanHurtPlayer => !IsDefeated && (!isStomped || (isKicked && kickGraceTimer <= 0f));

    public BuzzyBeetle(ISprite sprite, Vector2 initialPosition)
        : base(sprite, initialPosition, GameConstants.Entities.StandardSize,
            GameConstants.Entities.StandardSize)
    {
        Velocity = new Vector2(-GameConstants.Enemies.StandardWalkSpeed, 0f);
        // leftBound = initialPosition.X - patrolDistance;
        // rightBound = initialPosition.X + patrolDistance;
    }

    public override void HitByFireball()
    {
        return;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = facingDirection == Direction.Right
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;

        sprite.Draw(spriteBatch, Position, effects: effects);
    }

    public override void Update(GameTime gameTime)
    {
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
            kickGraceTimer = GameConstants.Enemies.KickGraceDuration;
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