using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2D.Interfaces
{
    public interface ISprite
    {
        void UpdateAnimation(GameTime gameTime);
        void Draw(
            SpriteBatch spriteBatch,
            Vector2 position,
            float rotation = 0f,
            SpriteEffects effects = SpriteEffects.None);
        void Play(string clipName);
    }
}