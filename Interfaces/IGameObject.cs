using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Interfaces;

public interface IGameObject
{
    public void Draw(SpriteBatch spriteBatch);
    public void Update(GameTime gameTime);
}