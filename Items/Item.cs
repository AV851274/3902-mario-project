using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game2D.Interfaces;

public class Item : IItem
{
    //A good portion of this is temporary, as a lot of functionality will be changed in later sprints.

    private List<ISprite> sprites;
    private int currentSpriteIndex = 0;

    public Vector2 Position { get; private set; }

    public Item(List<ISprite> sprites, Vector2 position)
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
        sprites[currentSpriteIndex].UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprites[currentSpriteIndex].Draw(spriteBatch, Position);
    }
}
