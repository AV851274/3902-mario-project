using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

    public void Update(GameTime gameTime)
    {
        // Static sprites do not have any update logic for now
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        Rectangle spritePosition = new Rectangle(
            (int)position.X,
            (int)position.Y,
            sourceRectangle.Width * scale,
            sourceRectangle.Height * scale);

        spriteBatch.Draw(texture, spritePosition, sourceRectangle, Color.White);
    }

}
