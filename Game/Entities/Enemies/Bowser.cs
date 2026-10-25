using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Game.Interfaces;
using Mario.Game.Animation;
using System;

namespace Mario.Game.Entities.Enemies;

public class Bowser : EnemyBase
{
    private const float WalkSpeed = 30f;
    private const float JumpSpeed = -500f;
    private const int Width = 80;
    private const int Height = 80;
    public override bool canBeStomped => false;

    private bool isActive = true;

    private Direction facingDirection = Direction.Left;

    public Direction FacingDirection
    {
        get { return facingDirection; }
    }

    public override bool IsDefeated => base.IsDefeated;

    public event Action<Vector2, Direction> SummonHammer;

    public event Action<Vector2, Direction> SummonFire;

    public Bowser(ISprite sprite, Vector2 initialPosition)
        : base(sprite, initialPosition, Width, Height)
    {
        IsOnGround = true;
        Velocity = new Vector2(-WalkSpeed, 0f);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!isActive)
            return;
        SpriteEffects effects = facingDirection == Direction.Right
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;

        if (IsDefeated)
        {
            effects = SpriteEffects.FlipVertically;
        }

        sprite.Draw(spriteBatch, Position, effects: effects);
    }

    public override void Update(GameTime gameTime)
    {
        if (!isActive)
            return;
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (!IsDefeated)
        {
            if (Random.Shared.NextDouble() < 0.05) // 5% chance to throw a hammer each frame
            {
                ThrowHammer();
            }

            if (Random.Shared.NextDouble() < 0.01) // 1% chance to breathe fire each frame
            {
                BreatheFire();
            }

            if (IsOnGround && Random.Shared.NextDouble() < 0.01) // 1% chance to jump each frame
            {
                Velocity = new Vector2(Velocity.X, JumpSpeed);
                IsOnGround = false;
            }

            if (Random.Shared.NextDouble() <
                0.05) // 5% chance to switch moving direction each frame without changin the facing direction
            {
                Velocity = new Vector2(-Velocity.X, Velocity.Y);
            }

        }
        else
        {
            Velocity = new Vector2(0f, Velocity.Y);
        }

        sprite.UpdateAnimation(gameTime);
    }


    public void ThrowHammer()
    {
        SummonHammer?.Invoke(Position, facingDirection);
    }

    public void BreatheFire()
    {
        SummonFire?.Invoke(Position, facingDirection);
    }

    protected override void OnWallBounce(float RelativeVelocityBeforeX)
    {
        Velocity = new Vector2(-RelativeVelocityBeforeX, Velocity.Y);
    }

    public override void Stomp()
    {
        return;
    }
}
