using System;
using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public class Spiny : EnemyBase
{
    private const float WalkSpeed = 60f;
    private const float KickSpeed = 6 * WalkSpeed;
    private const float KickGraceTime = 0.25f;
    private const int Width = 40;
    private const int Height = 40;

    // private float leftBound;
    // private float rightBound;
    private Direction facingDirection = Direction.Left;

    public Direction FacingDirection => facingDirection;

    public override bool CanHurtPlayer => !IsDefeated;

    public override bool canBeStomped => false;

    public Spiny(ISprite sprite, Vector2 initialPosition)
        : base(sprite, initialPosition, Width, Height)
    {
        Velocity = new Vector2(-WalkSpeed, 0f);
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
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        sprite.UpdateAnimation(gameTime);
    }

    public override void Stomp()
    {
        return;
    }

    protected override void OnWallBounce(float RelativeVelocityBeforeX)
    {
        //-collision.RelativeVelocityBefore.X
        Velocity = new Vector2(-RelativeVelocityBeforeX, Velocity.Y);
        facingDirection = Velocity.X < 0f ? Direction.Left : Direction.Right;
    }
}