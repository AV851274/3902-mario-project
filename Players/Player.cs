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

    private sealed record AppearanceKey(MarioPower Power, bool IsSwimming);

    private sealed record Appearance(ISprite Sprite, Vector2 Offset);

    private readonly Dictionary<AppearanceKey, Appearance> appearancesTable;
    private IPhysicsBody body;
    private Appearance appearance;
    private float timeSinceThrow = float.PositiveInfinity;
    private Direction facingDirection = Direction.Right;
    private bool isSwimming;


    public IPlayerBodyAction BodyAction => body as IPlayerBodyAction;
    public IMarioPowerState MarioPowerState { get; private set; } = MarioPowerStateFactory.Create(MarioPower.Small);
    public Vector2 Position => body.Position;
    public event Action<Vector2, Direction> SummonFireball;


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

    public void TakeDamage() => SetPower(MarioPowerState.TakeDamage());

    public void Crouch()
    {
        if (MarioPowerState.CanCrouch)
            BodyAction.Crouch();
    }

    public void Attack()
    {
        if (!MarioPowerState.CanShootFireball)
            return;

        SummonFireball?.Invoke(Position, facingDirection);
        timeSinceThrow = 0f;
    }

    public void Update(GameTime gameTime)
    {
        BodyAction.Update(gameTime);
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        timeSinceThrow += deltaTime;

        // TODO: Temporary integration until the shared physics engine is available.
        Vector2 position = body.Position;
        Vector2 velocity = body.Velocity;
        float gravity = 1000f;
        float? maxFallSpeed = null;
        if (isSwimming)
        {
            gravity = 200f;
            maxFallSpeed = 100f;
        }

        body.IsOnGround = GravityPhysics.Apply(ref position, ref velocity,
            deltaTime, gravity, 400f, maxFallSpeed);
        body.Position = position;
        body.Velocity = velocity;

        string animation;
        if (body is SwimmingPlayerBody swimmingBody)
        {
            facingDirection = swimmingBody.FacingDirection;
            if (timeSinceThrow < ThrowAnimationTime)
                animation = "Throw";
            else if (swimmingBody.IsSwimming)
                animation = "Swim";
            else if (!body.IsOnGround)
                animation = "Float";
            else if (velocity.X != 0f)
                animation = "Walk";
            else
                animation = "Stand";
        }
        else
        {
            var groundBody = (PlayerBody)body;
            facingDirection = groundBody.FacingDirection;
            if (!body.IsOnGround)
                animation = "Jump";
            else if (timeSinceThrow < ThrowAnimationTime)
                animation = "Throw";
            else if (groundBody.IsCrouching && MarioPowerState.CanCrouch)
                animation = "Crouch";
            else if (groundBody.IsSkidding)
                animation = "Skid";
            else if (velocity.X != 0f)
                animation = "Run";
            else
                animation = "Idle";
        }

        appearance.Sprite.Play(animation);
        appearance.Sprite.UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        bool flip = facingDirection == Direction.Right;
        if (isSwimming)
            flip = facingDirection == Direction.Left;
        SpriteEffects effects = flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        appearance.Sprite.Draw(spriteBatch, Position + appearance.Offset, effects: effects);
    }
}