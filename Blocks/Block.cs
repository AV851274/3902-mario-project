using Mario.Interfaces;
using Mario.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Blocks;

public class Block : IBlock
{
    private ISprite sprite;

    public Vector2 Position { get; private set; }

    public Block(SpriteFactory spriteFactory, BlockType type, Vector2 position)
    {
        sprite = spriteFactory.CreateBlockSprite(type);
        Position = position;
    }

    public void Update(GameTime gameTime)
    {
        sprite.UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, Position);
    }
}