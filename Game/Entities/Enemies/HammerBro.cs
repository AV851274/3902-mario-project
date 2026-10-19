using Mario.Game.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Game.Interfaces;
using Mario.Game.Animation;
using System;

namespace Mario.Game.Entities.Enemies;

public enum HammerBroState
{
    Idle,
    Thrown,
    Stuck
}

public class HammerBro : IEnemy, IPhysicsBody
{
    private const float WalkSpeed = 30f;
    private const float JumpSpeed = -500f;
    private const int Width = 32;
    private const int Height = 48;

    private ISprite sprite;
    private bool isStomped;
    private bool defeatedByFireball;

    private Direction facingDirection = Direction.Left;

    public Direction FacingDirection
    {
        get { return facingDirection; }
    }

    public Vector2 PendingAcceleration { get; set; }
    public bool IsOnGround { get; set; }
    public float GravityScale => 1f;
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }

    public Box Bounds => new Box(Position.X, Position.Y, Width, Height);
    public bool Active => Position.Y <= 1000f;
    public bool CollisionEnabled => Active && !IsDefeated;
    public bool IsDefeated => isStomped || defeatedByFireball;
    public bool CanHurtPlayer => !IsDefeated;

    public event Action<Vector2, Direction> SummonHammer;

    private HammerBroState state = HammerBroState.Idle;
    private float throwElapsed;
    private const float HammerThrowDuration = 0.4f;

    public HammerBro(ISprite sprite, Vector2 initialPosition)
    {
        this.sprite = sprite;
        Position = initialPosition;
        IsOnGround = true;
        Velocity = new Vector2(-WalkSpeed, 0f);
    }

    public void Draw(SpriteBatch spriteBatch)
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

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (!IsDefeated)
        {
            if (Random.Shared.NextDouble() < 0.01) // 1% chance to throw a hammer each frame
            {
                state = HammerBroState.Thrown;
                throwElapsed = 0;
                sprite.Play("HammerThrow");
                ThrowHammer();
            }

            if (IsOnGround && Random.Shared.NextDouble() < 0.02) // 2% chance to jump each frame
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
        if (defeatedByFireball)
            return;

        if (isStomped)
            return;

        sprite.Play("Stomped");
        isStomped = true;
        Velocity = new Vector2(0f, Velocity.Y);
    }

    public void HitByFireball()
    {
        if (IsDefeated)
            return;

        defeatedByFireball = true;
        Velocity = new Vector2(0f, -200f);
        IsOnGround = false;
    }

    public void OnCollision(in CollisionEvent collision)
    {
        if (collision.Other is IBlock && collision.Normal.X != 0f)
        {
            Velocity = new Vector2(-collision.RelativeVelocityBefore.X, Velocity.Y);
        }
    }
    public void ThrowHammer()
    {
        SummonHammer?.Invoke(Position, facingDirection);
    }
}
