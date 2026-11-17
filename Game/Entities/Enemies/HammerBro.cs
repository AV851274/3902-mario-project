using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Game.Interfaces;
using Mario.Game.Entities.Projectiles;
using Mario.Game.Animation;
using System;

namespace Mario.Game.Entities.Enemies;

public enum HammerBroState
{
    Idle,
    Thrown,
    Stuck
}

public class HammerBro : EnemyBase
{
    private readonly IEntitySpawner spawner;

    private const float HammerThrowChancePerFrame = 0.01f;
    private const float JumpChancePerFrame = 0.02f;

    private bool isStomped;

    private Direction facingDirection = Direction.Left;

    public Direction FacingDirection
    {
        get { return facingDirection; }
    }

    public override bool IsDefeated => isStomped || base.IsDefeated;


    private HammerBroState state = HammerBroState.Idle;
    private float throwElapsed;
    private const float HammerThrowDuration = 0.4f;

    public HammerBro(ISprite sprite, Vector2 initialPosition, IEntitySpawner spawner)
        : base(sprite, initialPosition, GameConstants.Entities.StandardSize,
            GameConstants.Entities.TallHeight)
    {
        this.spawner = spawner;
        IsOnGround = true;
        Velocity = new Vector2(-GameConstants.Enemies.SlowWalkSpeed, 0f);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
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
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (!IsDefeated)
        {
            if (Random.Shared.NextDouble() < HammerThrowChancePerFrame)
            {
                state = HammerBroState.Thrown;
                throwElapsed = 0;
                sprite.Play("HammerThrow");
                ThrowHammer();
            }

            if (IsOnGround && Random.Shared.NextDouble() < JumpChancePerFrame)
            {
                Velocity = new Vector2(Velocity.X, GameConstants.Enemies.StandardJumpVelocity);
                IsOnGround = false;
            }

            if (Random.Shared.NextDouble() <
                GameConstants.Enemies.DirectionChangeChancePerFrame)
            {
                Velocity = new Vector2(-Velocity.X, Velocity.Y);
            }

        }
        else
        {
            Velocity = new Vector2(0f, Velocity.Y);
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

    public override void Stomp()
    {
        if (DefeatedByFireball)
            return;

        if (isStomped)
            return;

        sprite.Play("Stomped");
        isStomped = true;
        Velocity = new Vector2(0f, Velocity.Y);
    }

    public void ThrowHammer()
    {
        float direction = facingDirection == Direction.Left ? -1f : 1f;
        spawner.TrySpawnProjectile(ProjectileType.Hammer,
            Position + new Vector2(
                GameConstants.Enemies.HammerSpawnOffsetX * direction,
                GameConstants.Enemies.HammerSpawnOffsetY),
            new Vector2(
                GameConstants.Enemies.ProjectileHorizontalVelocity * direction,
                GameConstants.Enemies.HammerVelocityY));
    }

    protected override void OnWallBounce(float RelativeVelocityBeforeX)
    {
        Velocity = new Vector2(-RelativeVelocityBeforeX, Velocity.Y);
    }
}
