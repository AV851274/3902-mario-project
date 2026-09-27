using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IProjectile
{
    Vector2 Position { get; }
    public bool Active { get; }

    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch);
}