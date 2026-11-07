using Mario.Game.Physics;
using System;
using System.Collections.Generic;
using Mario.Game.Animation;
using Mario.Game.Audio;
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
    private const float StompGraceDuration = 0.2f;
    private const float DeathJumpSpeed = 500f;
    private const float FallOffScreenY = 1000f;

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
    private float stompGraceTimer;
    private bool isDying;

    public IGameObject.DrawLayer layer => IGameObject.DrawLayer.Foreground;
    public event Action WarpRequested;
    private bool isHoldingDown;


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

    public float GravityScale
    {
        get
        {
            if (isDying)
                return 1f;

            return body.GravityScale;
        }
    }

    public float MaxFallSpeed
    {
        get
        {
            if (isDying)
                return float.PositiveInfinity;

            return body.MaxFallSpeed;
        }
    }


    public event Action<Vector2, Direction> SummonFireball;
    public event Action<SoundType> SoundRequested;
    public event Action DeathStarted;
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
    public bool CollisionEnabled => !isDying;
    public Box Bounds => body.Bounds;

    public Player(SpriteFactory factory, Vector2 position)
    {
        appearancesTable = new() {
            [new AppearanceKey(MarioPower.Small, false)] =
                new Appearance(factory.Create("mario", "small"), Vector2.Zero),
            [new AppearanceKey(MarioPower.Big, false)] =
                new Appearance(factory.Create("mario", "big"), new Vector2(0f, -40f)),
            [new AppearanceKey(MarioPower.Fire, false)] =
                new Appearance(factory.Create("mario", "fire"), new Vector2(0f, -40f)),
            [new AppearanceKey(MarioPower.Small, true)] =
                new Appearance(factory.Create("mario", "small.swimming"), Vector2.Zero),
            [new AppearanceKey(MarioPower.Big, true)] =
                new Appearance(factory.Create("mario", "big.swimming"), new Vector2(0f, -40f)),
            [new AppearanceKey(MarioPower.Fire, true)] =
                new Appearance(factory.Create("mario", "fire.swimming"), new Vector2(0f, -40f)),
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
        if (isDying)
            return;

        if (power == MarioPower.Dead)
        {
            StartDying();
            return;
        }

        if (power == MarioPowerState.Power)
            return;

        MarioPowerState = MarioPowerStateFactory.Create(power);
        RefreshSprite();
    }

    private void RefreshSprite()
    {
        appearance = appearancesTable[new AppearanceKey(MarioPowerState.Power, isSwimming)];
        timeSinceThrow = float.PositiveInfinity;
    }

    private void StartDying()
    {
        isDying = true;
        DeathStarted?.Invoke();
        appearance = appearancesTable[new AppearanceKey(MarioPower.Small, false)];
        Velocity = new Vector2(0f, -DeathJumpSpeed);
        IsOnGround = false;
        invincibleTimer = 0f;
    }

    public void TakeDamage()
    {
        // Briefly invincible after a hit, so one enemy touch only counts once
        if (isDying || invincibleTimer > 0f)
            return;

        SetPower(MarioPowerState.TakeDamage());
        invincibleTimer = InvincibleDuration;
    }

    public void OnCollision(CollisionEvent collision)
    {
        if (collision.Other is IBlock pipe && pipe.IsWarpPipe && collision.Normal.Y < 0f && isHoldingDown)
        {
            WarpRequested?.Invoke();
        }
        if (collision.Other is IEnemy enemy && !enemy.IsDefeated)
        {
            if (stompGraceTimer > 0f)
                return;

            bool stomped = (collision.Normal.Y < 0f && collision.RelativeVelocityBefore.Y > 0f)
                || Velocity.Y > 0f;
            if (stomped && enemy.canBeStomped)
            {
                enemy.Stomp();
                SoundRequested?.Invoke(SoundType.StompSwim);
                stompGraceTimer = StompGraceDuration;
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

    private SoundType JumpSound()
    {
        if (isSwimming)
        {
            return SoundType.StompSwim;
        }

        if (MarioPowerState.Power == MarioPower.Small)
        {
            return SoundType.SmallJump;
        }

        return SoundType.BigJump;
    }

    private void adjustBoundsForSprite()
    {
        bool isSmall = MarioPowerState.Power == MarioPower.Small;
        //Horizontal and vertical offset
        body.LocalBounds = new Box(0, isSmall ? 8 : body.IsCrouching ? -8: -32, 
        //Width
        isSmall && (body.IsOnGround || isSwimming) ? ((body.IsOnGround && isSwimming && body.Velocity.X == 0f) ? 32 
        : (body.IsOnGround && isSwimming) ? 36 : (body.IsOnGround && body.Velocity.X != 0f) ? 42 : 34) : 42, 
        //Height
        isSmall ? 40 : (body.IsCrouching ? 56 : 80));
    }

    public void Crouch()
    {
        isHoldingDown = true;
        if (MarioPowerState.CanCrouch)
            body.Crouch();
    }

    public void Attack()
    {
        attackPressed = true;
    }

    public void Update(GameTime gameTime)
    {
        adjustBoundsForSprite();

        if (Position.Y > FallOffScreenY)
        {
            // Falling into a pit skips the death animation, so the death starts here
            if (!isDying)
            {
                DeathStarted?.Invoke();
            }
            Died?.Invoke();
            return;
        }

        if (isDying)
            return;

        body.Update(gameTime);
        if (body.JumpedThisFrame)
        {
            SoundRequested?.Invoke(JumpSound());
        }
        facingDirection = body.FacingDirection;
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        timeSinceThrow += deltaTime;
        invincibleTimer = Math.Max(0f, invincibleTimer - deltaTime);
        stompGraceTimer = Math.Max(0f, stompGraceTimer - deltaTime);

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
        isHoldingDown = false;
        if (isDying)
        {
            appearance.Sprite.Play("Dead");
            appearance.Sprite.UpdateAnimation(gameTime);
            return;
        }

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
