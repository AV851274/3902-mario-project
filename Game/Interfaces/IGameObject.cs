using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Interfaces;

public interface IGameObject
{
    public enum DrawLayer
    {
        Background, 
        Block, 
        Foreground,
        Effects
    }
    public DrawLayer layer {get; }
    public void Draw(SpriteBatch spriteBatch);
    public void Update(GameTime gameTime);
    public bool Active => true;
}