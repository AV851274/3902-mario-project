using Mario.Game.Interfaces;
using Mario.Game.Animation;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public class CheepCheep : EnemyBase
{
    public override bool IsDefeated => StruckByFireball;
    public override bool canBeStomped => false;
    public override ICollider.BlockingModes BlockingMode => ICollider.BlockingModes.ThroughWalls;
    public override float GravityScale => StruckByFireball ? 0.2f : 0f;
    private float SwimSpeed = 60f;
    private const int Size = 40;
    private const float bobHeight = 5f;
    private const float boundSize = 75f;
    private const float turnAcc = 1f;
    private float targetVelocityX;
    private float bobTimer = 0f;
    private float bobDuration => 40f/SwimSpeed; // Time it takes to complete one bob cycle
    private float leftBound;
    private float rightBound;
    private bool StruckByFireball = false;
    private Direction facingDirection = Direction.Left;
    public Direction FacingDirection => facingDirection;
    private FishColor fishColor;

    public CheepCheep(ISprite sprite, Vector2 initialPosition, FishColor color)
        : base(sprite, initialPosition, Size, Size)
    {
        fishColor = color;
        if (color == FishColor.Green)
        {
            SwimSpeed *= 1.5f;
            leftBound = initialPosition.X - boundSize;
            rightBound = initialPosition.X + boundSize;
        }
            
        Velocity = new Vector2(-SwimSpeed, 0f);
        targetVelocityX = -SwimSpeed;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = facingDirection == Direction.Right
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;

        if (IsDefeated)
            effects |= SpriteEffects.FlipVertically;

        sprite.Draw(spriteBatch, Position - new Vector2(0, BobOffset()), effects: effects);
    }

    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;         

        if (IsDefeated)
        {
            Velocity = new Vector2(0f, Velocity.Y);
        } else
        {
            bobTimer += deltaTime;
            if (fishColor == FishColor.Green) adjustGreenPosition();
        }

        sprite.UpdateAnimation(gameTime);
    }

    public override void Stomp()
    {

    }

    // protected override void OnWallBounce(float RelativeVelocityBeforeX)
    // {

    // }

    public override void HitByFireball()
    {
        StruckByFireball = true;
        Velocity = new Vector2(0f, -100f);
    }

    private float BobOffset()
    {
        float progress = bobTimer / bobDuration;
        return MathF.Sin(progress * MathF.PI) * bobHeight;
    }

    private float adjustGreenPosition()
    {
        if (Position.X < leftBound)
        {
            targetVelocityX = SwimSpeed;
            //Position = new Vector2(leftBound, Position.Y);
            Velocity = new Vector2(Velocity.X+turnAcc, Velocity.Y);
            if (Velocity.X > targetVelocityX) Velocity = new Vector2(targetVelocityX, Velocity.Y);
        }
        else if (Position.X > rightBound)
        {
            targetVelocityX = -SwimSpeed;
            //Position = new Vector2(rightBound, Position.Y);
            Velocity = new Vector2(Velocity.X-turnAcc, Velocity.Y);
            if (Velocity.X < targetVelocityX) Velocity = new Vector2(targetVelocityX, Velocity.Y);
        }

        facingDirection = Velocity.X > 0 ? Direction.Right : Direction.Left;

        return Position.X;
    }
}