using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IBlock
{
    Vector2 Position { get; }

    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch);

    void nextSprite();

    void prevSprite();

    //TODO: Collision next sprint, Bump(), Break(), etc.)
}