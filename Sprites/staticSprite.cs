using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game2D.Interfaces;

public class StaticSprite : ISprite
{

    private Texture2D texture;
    private Rectangle sourceRectangle;
    private int scale = 3;

    public StaticSprite(Texture2D texture, Rectangle sourceRectangle)
    {
        this.texture = texture;
        this.sourceRectangle = sourceRectangle;
    }

    public void UpdateAnimation(GameTime gameTime)
    {
        // Static sprites do not have any update logic for now
    }

    public void Play(string clipName)
    {
    }

    public void Draw(
        SpriteBatch spriteBatch,
        Vector2 position,
        float rotation = 0f,
        SpriteEffects effects = SpriteEffects.None)
    {
        spriteBatch.Draw(
            texture,
            position,
            sourceRectangle,
            Color.White,
            rotation,
            Vector2.Zero,
            scale,
            effects,
            0f);
    }

}
