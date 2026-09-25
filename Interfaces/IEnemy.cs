using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IEnemy
{
    Vector2 Position { get; }

    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch);

    //void Stomp();

    //TODO: Collision next sprint (hit by fireball, shell, etc.)
}