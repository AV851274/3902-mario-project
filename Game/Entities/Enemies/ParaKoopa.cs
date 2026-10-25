using System;
using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public class ParaKoopa : EnemyBase
{
    private const float WalkSpeed = 60f;
    private const int Width = 40;
    private const int Height = 60;
    private const float JumpSpeed = -500f;

    // private float leftBound;
    // private float rightBound;
    private Direction facingDirection = Direction.Left;

    public Direction FacingDirection => facingDirection;

    public event Action<ParaKoopa> Stomped;

    // Walking Koopas and moving shells hurt; a resting shell (or one just kicked) doesn't
    public override bool CanHurtPlayer => !IsDefeated;

    public ParaKoopa(ISprite sprite, Vector2 initialPosition)
        : base(sprite, initialPosition, Width, Height)
    {
        Velocity = new Vector2(-WalkSpeed, 0f);
        IsOnGround = true;
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
        if (IsOnGround)
        {
            Velocity = new Vector2(Velocity.X, JumpSpeed);
            IsOnGround = false;
        }

        sprite.UpdateAnimation(gameTime);
    }

    public override void HitByFireball()
    {
        Stomp();
    }

    public override void Stomp()
    {
        Stomped?.Invoke(this);
    }

    protected override void OnWallBounce(float RelativeVelocityBeforeX)
    {
        //-collision.RelativeVelocityBefore.X
        Velocity = new Vector2(-RelativeVelocityBeforeX, Velocity.Y);
        facingDirection = Velocity.X < 0f ? Direction.Left : Direction.Right;
    }
}