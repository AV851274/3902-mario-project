using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Interfaces;

public interface IGameObject
{
    public void Draw(SpriteBatch spriteBatch);
    public void Update(GameTime gameTime);
}