using Mario.Game.Physics;
using System;
using Mario.Game.Animation;
using Mario.Game.Audio;
using Mario.Game.Interfaces;
using Mario.Game.Interfaces.Player;
using Mario.Game.Entities.Players.PlayerStates;
using Mario.Game.Entities.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Players;

public class Player : IPlayer
{
    private readonly IEntitySpawner spawner;
    private const float InvincibleDuration = 1.5f;
    private const float StompGraceDuration = 0.2f;
    private const float DeathJumpSpeed = 500f;
    private const float FallOffScreenY = 1000f;

    private readonly PlayerAppearance appearance;
    private PlayerBodyBase body;
    private Direction facingDirection = Direction.Right;
    private bool isSwimming;
    private bool attackPressed;
    private float invincibleTimer;
    private float stompGraceTimer;
    private bool isDying;

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
        get {
            if (isDying)
                return 1f;

            return body.GravityScale;
        }
    }

    public float MaxFallSpeed
    {
        get {
            if (isDying)
                return float.PositiveInfinity;

            return body.MaxFallSpeed;
        }
    }


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

    public Player(ISpriteFactory sprites, IEntitySpawner spawner, Vector2 position)
    {
        this.spawner = spawner;
        appearance = new PlayerAppearance(sprites);
        body = new PlayerBody { Position = position };
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
        appearance.SetForm(MarioPowerState.Power, isSwimming);
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
        appearance.SetForm(MarioPowerState.Power, isSwimming);
    }

    private void StartDying()
    {
        isDying = true;
        DeathStarted?.Invoke();
        appearance.ShowDeath();
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
            return SoundType.StompSwim;
        if (MarioPowerState.Power == MarioPower.Small)
            return SoundType.SmallJump;
        return SoundType.BigJump;
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
        body.UpdateBounds(MarioPowerState.Power);

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
        appearance.Update(gameTime);
        invincibleTimer = Math.Max(0f, invincibleTimer - deltaTime);
        stompGraceTimer = Math.Max(0f, stompGraceTimer - deltaTime);

        // Attack
        if (attackPressed && MarioPowerState.CanShootFireball)
        {
            ShootFireball();
            appearance.PlayThrow();
        }

        attackPressed = false;
    }

    private void ShootFireball()
    {
        float direction = facingDirection == Direction.Left ? -1f : 1f;
        if (spawner.TrySpawnProjectile(ProjectileType.Fireball,
            Position + new Vector2(25f * direction, -20f),
            new Vector2(500f * direction, 200f)))
        {
            SoundRequested?.Invoke(SoundType.Fireball);
        }
    }

    public void UpdateAnimation(GameTime gameTime)
    {
        isHoldingDown = false;
        appearance.UpdateAnimation(gameTime, body, MarioPowerState.CanCrouch, isDying);
    }

    public void Draw(SpriteBatch spriteBatch) =>
        appearance.Draw(spriteBatch, body, facingDirection, invincibleTimer);
}
