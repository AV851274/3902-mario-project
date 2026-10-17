using System;
using System.Collections.Generic;
using Mario.Animation;
using Mario.Interfaces;
using Mario.Interfaces.Player;
using Mario.Physics;
using Mario.Players.PlayerStates;
using Mario.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Players;

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
    public IMarioPowerState MarioPowerState { get; private set; } = MarioPowerStateFactory.Create(MarioPower.Small);
    public Vector2 Position => body.Position;
    public event Action<Vector2, Direction> SummonFireball;
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

    public void SetPosition(Vector2 position)
    {
        body.Position = position;
    }
    public Rectangle Bounds
    {
        get
        {
            bool isSmall = MarioPowerState.Power == MarioPower.Small;

            int width = 36;
            int height = isSmall ? 48 : 88;

            int x = (int)Position.X;
            int y = (int)Position.Y;

            if (!isSmall)
            {
                y -= 40;
            }

            return new Rectangle(x, y, width, height);
        }
    }


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
        // Death behavior will be implemented later.
        if (power == MarioPower.Dead || power == MarioPowerState.Power)
            return;

        MarioPowerState = MarioPowerStateFactory.Create(power);
        RefreshSprite();
    }

    private void RefreshSprite()
    {
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
        body.Update(gameTime);
        facingDirection = body.FacingDirection;
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        timeSinceThrow += deltaTime;
        invincibleTimer = Math.Max(0f, invincibleTimer - deltaTime);

        // TODO: Temporary integration until the shared physics engine is available.
        Vector2 position = body.Position;
        Vector2 velocity = body.Velocity;
        float gravity = 1000f;
        float maxFallSpeed = 0f;
        if (isSwimming)
        {
            gravity = 200f;
            maxFallSpeed = 100f;
        }

        GravityPhysics.Apply(ref position, ref velocity, deltaTime, gravity, maxFallSpeed);

        body.IsOnGround = false;
        body.Position = position;
        body.Velocity = velocity;

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
