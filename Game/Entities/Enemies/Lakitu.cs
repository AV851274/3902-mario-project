using System;
using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Mario.Game.Interfaces.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;
public enum LakituState
{
    Idle,
    Thrown
}
public class Lakitu : EnemyBase
{
    private const float FollowSpeed = 800f;
    private const float FollowAcceleration = 200f;
    private const float FollowDistance = 120f;
    private const float LeadTime = 0.3f;
    private const int Width = 40;
    private const int Height = 60;

    private const float EggThrowDuration = 0.4f;

    private LakituState state = LakituState.Idle;
    private float throwElapsed;
    private Direction facingDirection = Direction.Left;
    private IPlayer target;

    private bool wasEggThrown = false;

    public override float GravityScale => DefeatedByFireball ? 1f : 0f;

    public Direction FacingDirection => facingDirection;

    public event Action<Vector2, Direction> SummonEgg;

    public void Follow(IPlayer player)
    {
        target = player;
    }

    public Lakitu(ISprite sprite, Vector2 initialPosition)
        : base(sprite, initialPosition, Width, Height)
    {
        Velocity = Vector2.Zero;
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

        UpdateFollowVelocity(deltaTime);

        if (state == LakituState.Idle && Random.Shared.NextDouble() < 0.005)
        {
            state = LakituState.Thrown;
            throwElapsed = 0;
            sprite.Play("EggThrow");
        }

        if (state == LakituState.Thrown && throwElapsed >= EggThrowDuration)
        {
            state = LakituState.Idle;
            throwElapsed = 0;
            sprite.Play("Idle");
            wasEggThrown = false;
        }
        else
        {
            if (!wasEggThrown && throwElapsed >= EggThrowDuration / 2f && state == LakituState.Thrown)
            {
                wasEggThrown = true;
                ThrowEgg();
            }
            throwElapsed += deltaTime;
        }
        sprite.UpdateAnimation(gameTime);
    }

    private void UpdateFollowVelocity(float deltaTime)
    {
        if (target is null)
            return;

        float followX = target.Position.X + FollowDistance +
            Math.Max(0f, target.Velocity.X * LeadTime);
        float offsetX = followX - Position.X;
        float targetSpeed = Math.Clamp(offsetX * 2f, -FollowSpeed, FollowSpeed);
        float speedChange = Math.Clamp(
            targetSpeed - Velocity.X,
            -FollowAcceleration * deltaTime,
            FollowAcceleration * deltaTime);
        Velocity = new Vector2(Velocity.X + speedChange, 0f);

        if (Velocity.X != 0f)
            facingDirection = Velocity.X < 0f ? Direction.Left : Direction.Right;
    }

    public void ThrowEgg()
    {
        SummonEgg?.Invoke(Position, facingDirection);
    }

    public override void Stomp()
    {
        if (DefeatedByFireball)
            return;

        base.HitByFireball();
    }

    protected override void OnWallBounce(float RelativeVelocityBeforeX)
    {
        //-collision.RelativeVelocityBefore.X
        Velocity = new Vector2(-RelativeVelocityBeforeX, Velocity.Y);
        facingDirection = Velocity.X < 0f ? Direction.Left : Direction.Right;
    }
}