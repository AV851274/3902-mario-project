using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Interfaces;

public interface IItem
{
    Vector2 Position { get; }

    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch);
}
