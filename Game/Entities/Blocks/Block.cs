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
    private float disappearTimer = -100f;
    private float cloudDisappearTime = 2f;
    public bool Active => disappearTimer > 0f || disappearTimer < -50f;

    public Vector2 Position { get; private set; }
    public bool IsSolid => true;
    public bool isSemisolid => type == BlockType.Bridge || type == BlockType.Cloud || type == BlockType.MovingPlatform;
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
        if (type == BlockType.Cloud && collision.Normal.Y < 0f && 
        (collision.Other is IPlayer || collision.Other is IEnemy) && disappearTimer < -75f 
        && collision.RelativeVelocityBefore.Y < 0f)
        {
            disappearTimer = cloudDisappearTime;
        } else
        {
            return;
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
        handleTimer(deltaTime);

        sprite.UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
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

    private void handleTimer(float deltaTime)
    {
        if (disappearTimer > 0f)
        {
            disappearTimer -= deltaTime;
        }
    }

}
