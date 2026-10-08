using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Interfaces;

public interface IBlock
{
    Vector2 Position { get; }

    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch);

    //TODO: Collision next sprint, Bump(), Break(), etc.)
}
