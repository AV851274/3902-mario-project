using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

    public void nextSprite()
    {
        currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Count;
    }

    public void prevSprite()
    {
        currentSpriteIndex = (currentSpriteIndex - 1 + sprites.Count) % sprites.Count;
    }

    public void Update(GameTime gameTime)
    {
        sprites[currentSpriteIndex].Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprites[currentSpriteIndex].Draw(spriteBatch, Position);
    }
}