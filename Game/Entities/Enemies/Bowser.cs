using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Game.Interfaces;
using Mario.Game.Entities.Projectiles;
using Mario.Game.Animation;
using System;

namespace Mario.Game.Entities.Enemies;

public class Bowser : EnemyBase
{
    private readonly IEntitySpawner spawner;

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



    public Bowser(ISprite sprite, Vector2 initialPosition, IEntitySpawner spawner)
        : base(sprite, initialPosition, Width, Height)
    {
        this.spawner = spawner;
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
        float direction = facingDirection == Direction.Left ? -1f : 1f;
        spawner.TrySpawnProjectile(ProjectileType.Hammer,
            Position + new Vector2(20f * direction, -20f),
            new Vector2(250f * direction, -350f));
    }

    public void BreatheFire()
    {
        float direction = facingDirection == Direction.Left ? -1f : 1f;
        spawner.TrySpawnProjectile(ProjectileType.Fire,
            Position + new Vector2(20f * direction, 0f),
            new Vector2(250f * direction, 0f));
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
