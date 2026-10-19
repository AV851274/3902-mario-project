using Mario.Game.Physics;
using System;
using Mario.Game.Interfaces;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Game.Interfaces.Player;

namespace Mario.Game.Entities.Blocks;

public class Block : IBlock
{
    private const int Size = 48;
    private const float BumpHeight = 12f;
    private const float BumpDuration = 0.15f;

    private SpriteFactory spriteFactory;
    private ISprite sprite;
    private BlockType type;
    private float bumpTimer;
    private float cloudDisappearTime = 2f;
    private float cloudComebackTime = -2f;
    private bool cloudTimerStarted = false;
    private float cloudTimer = 0;
    public bool Active => true;


    public Vector2 Position { get; private set; }

    public ICollider.BlockingModes BlockingMode
    {
        get {
            if (type == BlockType.Cloud)
            {
                return cloudTimerStarted && cloudTimer <= 0f
                    ? ICollider.BlockingModes.None
                    : ICollider.BlockingModes.TopOnly;
            }

            if (type == BlockType.MovingPlatform || type == BlockType.Bridge)
                return ICollider.BlockingModes.TopOnly;

            return ICollider.BlockingModes.Solid;
        }
    }

    public Box Bounds => new Box(Position.X, Position.Y, Size, Size);

    public Block(SpriteFactory spriteFactory, BlockType type, Vector2 position)
    {
        this.spriteFactory = spriteFactory;
        this.type = type;
        sprite = spriteFactory.CreateBlockSprite(type);
        Position = position;
    }

    public void OnCollision(CollisionEvent collision)
    {
        //Set timer to despawn cloud if it's been landed on.
        if (type == BlockType.Cloud &&
            collision.Normal.Y > 0f &&
            (collision.Other is IPlayer || collision.Other is IEnemy) &&
            !cloudTimerStarted &&
            collision.RelativeVelocityBefore.Y < 0f)
        {
            cloudTimerStarted = true;
            cloudTimer = cloudDisappearTime;
        }

        //bool stomped = collision.Normal.Y < 0f && collision.RelativeVelocityBefore.Y > 0f;
    }

    public void Hit()
    {
        if (type != BlockType.Brick && type != BlockType.Question)
        {
            return;
        }

        bumpTimer = BumpDuration;

        if (type == BlockType.Question)
        {
            type = BlockType.UsedQuestion;
            sprite = spriteFactory.CreateBlockSprite(BlockType.UsedQuestion);
        }
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        bumpTimer = Math.Max(0f, bumpTimer - deltaTime);

        if (cloudTimerStarted)
            cloudTimer -= deltaTime;

        if (cloudTimerStarted && cloudTimer <= cloudComebackTime)
        {
            cloudTimerStarted = false;
            cloudTimer = 0;
        }

        sprite.UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (cloudTimerStarted && cloudTimer <= 0f)
            return;
        sprite.Draw(spriteBatch, Position - new Vector2(0, BumpOffset()));
    }

    private float BumpOffset()
    {
        if (bumpTimer <= 0f)
        {
            return 0f;
        }

        float progress = 1f - (bumpTimer / BumpDuration);
        return MathF.Sin(progress * MathF.PI) * BumpHeight;
    }
}