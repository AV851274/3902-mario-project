using Mario.Game.Physics;
using System;
using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public class Turtle : IEnemy, IPhysicsBody
{
    private const float WalkSpeed = 60f;
    private const float KickSpeed = 6 * WalkSpeed;
    private const float KickGraceTime = 0.25f;
    private const int Width = 32;
    private const int Height = 48;

    private ISprite sprite;
    private float leftBound;
    private float rightBound;
    private bool isStomped;
    private bool defeatedByFireball;
    private bool isKicked;
    private float kickGraceTimer;
    private Direction facingDirection = Direction.Left;

    public Direction FacingDirection => facingDirection;
    public Vector2 PendingAcceleration { get; set; }
    public bool IsOnGround { get; set; }
    public float GravityScale => 1f;
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Box Bounds => new Box(Position.X, Position.Y, Width, Height);
    public bool Active => Position.Y <= 1000f;
    public bool CollisionEnabled => Active && !IsDefeated;

    // Stomping leaves a shell; fireballs defeat it outright.
    public bool IsDefeated => defeatedByFireball;

    // Walking Koopas and moving shells hurt; a resting shell (or one just kicked) doesn't
    public bool CanHurtPlayer => !IsDefeated && (!isStomped || (isKicked && kickGraceTimer <= 0f));

    public Turtle(ISprite sprite, Vector2 initialPosition, int patrolDistance = 150)
    {
        this.sprite = sprite;
        Position = initialPosition;
        Velocity = new Vector2(-WalkSpeed, 0f);
        leftBound = initialPosition.X - patrolDistance;
        rightBound = initialPosition.X + patrolDistance;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = facingDirection == Direction.Right
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;

        if (defeatedByFireball)
            effects |= SpriteEffects.FlipVertically;

        sprite.Draw(spriteBatch, Position, effects: effects);
    }

    public void Update(GameTime gameTime)
    {
        if (defeatedByFireball)
        {
            sprite.UpdateAnimation(gameTime);
            return;
        }

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        kickGraceTimer = Math.Max(0f, kickGraceTimer - deltaTime);

        if (!isStomped || isKicked)
        {
            //TURN TURTLE AROUND WHEN IT HITS THE EDGE OF ITS PATROL AREA
            if (Position.X <= leftBound || Position.X >= rightBound)
            {
                Velocity = new Vector2(-Velocity.X, Velocity.Y);
                facingDirection = Velocity.X < 0
                    ? Direction.Left
                    : Direction.Right;
            }
        }

        if (isStomped && !isKicked)
            Velocity = new Vector2(0f, Velocity.Y);
        else if (isKicked && Velocity.X == 0f)
            Velocity = new Vector2(KickSpeed *
                (facingDirection == Direction.Left ? -1 : 1), Velocity.Y);

        sprite.UpdateAnimation(gameTime);
    }

    public void Stomp()
    {
        if (defeatedByFireball)
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
            facingDirection = Velocity.X < 0f ? Direction.Left : Direction.Right;
        }
    }
}
