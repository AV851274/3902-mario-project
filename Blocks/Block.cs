using Mario.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Mario.Blocks;

public class Block : IBlock
{
    private List<ISprite> sprites;
    private int currentSpriteIndex = 0;

    public Vector2 Position { get; private set; }

    public Block(List<ISprite> sprites, Vector2 position)
    {
        this.sprites = sprites;
        Position = position;
    }

    public void NextSprite()
    {
        currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Count;
    }

    public void PrevSprite()
    {
        currentSpriteIndex = (currentSpriteIndex - 1 + sprites.Count) % sprites.Count;
    }

    public void Update(GameTime gameTime)
    {
        sprites[currentSpriteIndex].UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprites[currentSpriteIndex].Draw(spriteBatch, Position);
    }
}
