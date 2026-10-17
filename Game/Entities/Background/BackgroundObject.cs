using Mario.Game.Interfaces;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Background;

public class BackgroundObject : IGameObject
{
    private ISprite sprite;

    public Vector2 Position { get; }

    public BackgroundObject(SpriteFactory spriteFactory, BackgroundType type, Vector2 position)
    {
        sprite = spriteFactory.CreateBackgroundSprite(type);
        Position = position;
    }

    public void Update(GameTime gameTime)
    {
        sprite.UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, Position);
    }
}