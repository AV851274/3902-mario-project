using Mario.Game.Physics;
using System;
using System.Collections.Generic;
using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Mario.Game.Interfaces.Player;
using Mario.Game.Entities.Players.PlayerStates;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Players;

public class Player : IPlayer
{
    private const float ThrowAnimationTime = 0.3f;
    private const float InvincibleDuration = 1.5f;

    private sealed record AppearanceKey(MarioPower Power, bool IsSwimming);

    private sealed record Appearance(ISprite Sprite, Vector2 Offset);

    private readonly Dictionary<AppearanceKey, Appearance> appearancesTable;
    private PlayerBodyBase body;
    private Appearance appearance;
    private float timeSinceThrow = float.PositiveInfinity;
    private Direction facingDirection = Direction.Right;
    private bool isSwimming;
    private bool attackPressed;
    private float invincibleTimer;


    public IPlayerBodyAction BodyAction => body;

    public IMarioPowerState MarioPowerState { get; private set; }
        = MarioPowerStateFactory.Create(MarioPower.Small);

    // Delegate Implementation of IPhysicsBody to body
    public Vector2 Position
    {
        get => body.Position;
        set => body.Position = value;
    }

    public Vector2 PendingAcceleration
    {
        get => body.PendingAcceleration;
        set => body.PendingAcceleration = value;
    }

    public float GravityScale => body.GravityScale;
    public float MaxFallSpeed => body.MaxFallSpeed;


    public event Action<Vector2, Direction> SummonFireball;
    public event Action Died;

    public Vector2 Velocity
    {
        get => body.Velocity;
        set => body.Velocity = value;
    }

    public bool IsOnGround
    {
        get => body.IsOnGround;
        set => body.IsOnGround = value;
    }

    public ICollider.BlockingModes BlockingMode => body.BlockingMode;
    public Box Bounds => body.Bounds;

    public Player(SpriteFactory factory, Vector2 position)
    {
        appearancesTable = new() {
            [new AppearanceKey(MarioPower.Small, false)] =
                new Appearance(factory.CreateSmallMarioSprite(), Vector2.Zero),
            [new AppearanceKey(MarioPower.Big, false)] =
                new Appearance(factory.CreateBigMarioSprite(), new Vector2(0f, -40f)),
            [new AppearanceKey(MarioPower.Fire, false)] =
                new Appearance(factory.CreateFireMarioSprite(), new Vector2(0f, -40f)),
            [new AppearanceKey(MarioPower.Small, true)] =
                new Appearance(factory.CreateSmallSwimmingMarioSprite(), Vector2.Zero),
            [new AppearanceKey(MarioPower.Big, true)] =
                new Appearance(factory.CreateBigSwimmingMarioSprite(), new Vector2(0f, -40f)),
            [new AppearanceKey(MarioPower.Fire, true)] =
                new Appearance(factory.CreateFireSwimmingMarioSprite(), new Vector2(0f, -40f)),
        };
        body = new PlayerBody { Position = position };
        RefreshSprite();
    }

    public void SetSwimming(bool swimming)
    {
        if (isSwimming == swimming)
            return;

        if (swimming)
            body = new SwimmingPlayerBody(body);
        else
            body = new PlayerBody(body);
        body.Velocity = Vector2.Zero;
        isSwimming = swimming;
        RefreshSprite();
    }

    public void SetPower(MarioPower power)
    {
        if (power == MarioPower.Dead)
        {
            Died?.Invoke();
            return;
        }

        if (power == MarioPowerState.Power)
            return;

        MarioPowerState = MarioPowerStateFactory.Create(power);
        RefreshSprite();
    }

    private void RefreshSprite()
    {
        bool isSmall = MarioPowerState.Power == MarioPower.Small;
        body.LocalBounds = new Box(0, isSmall ? 0 : -40, 36, isSmall ? 48 : 88);
        appearance = appearancesTable[new AppearanceKey(MarioPowerState.Power, isSwimming)];
        timeSinceThrow = float.PositiveInfinity;
    }

    public void TakeDamage()
    {
        // Briefly invincible after a hit, so one enemy touch only counts once
        if (invincibleTimer > 0f)
            return;

        SetPower(MarioPowerState.TakeDamage());
        invincibleTimer = InvincibleDuration;
    }

    public void OnCollision(CollisionEvent collision)
    {
        if (collision.Other is IEnemy enemy && !enemy.IsDefeated)
        {
            bool stomped = collision.Normal.Y < 0f && collision.RelativeVelocityBefore.Y > 0f;
            if (stomped && enemy.canBeStomped)
            {
                enemy.Stomp();
                Velocity = new Vector2(Velocity.X, -350f);
                IsOnGround = false;
            }
            else if (enemy.CanHurtPlayer)
                TakeDamage();
        }
        else if (collision.Other is IItem item && !item.IsCollected)
            item.Collect(this);
        else if (collision.Other is IBlock block && collision.Normal.Y > 0f &&
                 collision.RelativeVelocityBefore.Y < 0f) 
            block.Hit(MarioPowerState.CanCrouch);
    }

    public void Crouch()
    {
        if (MarioPowerState.CanCrouch)
            body.Crouch();
    }

    public void Attack()
    {
        attackPressed = true;
    }

    public void Update(GameTime gameTime)
    {
        if (Position.Y > 1000f)
        {
            SetPower(MarioPower.Dead);
            return;
        }

        body.Update(gameTime);
        facingDirection = body.FacingDirection;
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        timeSinceThrow += deltaTime;
        invincibleTimer = Math.Max(0f, invincibleTimer - deltaTime);

        // Attack
        if (attackPressed && MarioPowerState.CanShootFireball)
        {
            SummonFireball?.Invoke(Position, facingDirection);
            timeSinceThrow = 0f;
        }

        attackPressed = false;
    }

    public void UpdateAnimation(GameTime gameTime)
    {
        string animation;

        if (body is SwimmingPlayerBody swimmingBody)
        {
            if (timeSinceThrow < ThrowAnimationTime)
                animation = "Throw";
            else if (swimmingBody.IsSwimming)
                animation = "Swim";
            else if (!body.IsOnGround)
                animation = "Float";
            else if (body.Velocity.X != 0f)
                animation = "Walk";
            else
                animation = "Stand";
        }
        else
        {
            var groundBody = (PlayerBody)body;

            if (!body.IsOnGround)
                animation = "Jump";
            else if (timeSinceThrow < ThrowAnimationTime)
                animation = "Throw";
            else if (groundBody.IsCrouching && MarioPowerState.CanCrouch)
                animation = "Crouch";
            else if (groundBody.IsSkidding)
                animation = "Skid";
            else if (body.Velocity.X != 0f)
                animation = "Run";
            else
                animation = "Idle";
        }

        appearance.Sprite.Play(animation);
        appearance.Sprite.UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Flash while invincible: skip drawing every other tenth of a second
        if (invincibleTimer > 0f && (int)(invincibleTimer * 10) % 2 == 0)
            return;

        bool flip = facingDirection == Direction.Right;
        if (isSwimming)
            flip = facingDirection == Direction.Left;
        SpriteEffects effects = flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        Vector2 drawPosition = Position + appearance.Offset;

        if (appearance.Sprite is AnimationController animation)
        {
            drawPosition.Y = Bounds.Bottom - animation.CurrentFrameHeight;
        }

        appearance.Sprite.Draw(spriteBatch, drawPosition, effects: effects);
    }
}
