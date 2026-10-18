using System;
using Mario.Game.Interfaces;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

    public Vector2 Position { get; private set; }
    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Size, Size);

    public Block(SpriteFactory spriteFactory, BlockType type, Vector2 position)
    {
        this.spriteFactory = spriteFactory;
        this.type = type;
        sprite = spriteFactory.CreateBlockSprite(type);
        Position = position;
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
}