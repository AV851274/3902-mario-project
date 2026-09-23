using Microsoft.Xna.Framework;

public interface IItem
{
    
    Vector2 Position { get; }

    void Update(GameTime gameTime);

    void Draw(GameTime gameTime);

    void nextSprite();

    void prevSprite();

    //TODO: Next sprint, add effects like fire powerup, star powerup, etc. Lack of collisions makes this fine for now.

}