using Mario.Game.Entities.Background;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mario.Game.Rendering.Sprites;

public class HitboxSpriteFactory
{
    private readonly Texture2D tileSet;
    private readonly Rectangle tileSource = new(323, 26, 1, 1);
    public HitboxSpriteFactory(ContentManager content)
    {
        tileSet = content.Load<Texture2D>("tileset");
    }

    public void DrawHitbox(SpriteBatch spriteBatch, float left, float right, float top, float bottom)
    {
        Rectangle Top = new Rectangle((int)left, (int)top, (int)(right - left), 1);
        Rectangle Bottom = new Rectangle((int)left, (int)bottom-1, (int)(right - left), 1);
        Rectangle Left = new Rectangle((int)left, (int)top, 1, (int)(bottom - top));
        Rectangle Right = new Rectangle((int)right-1, (int)top, 1, (int)(bottom - top));

        spriteBatch.Draw(tileSet, Top, tileSource, Color.Red);
        spriteBatch.Draw(tileSet, Bottom, tileSource, Color.Red);
        spriteBatch.Draw(tileSet, Left, tileSource, Color.Red);
        spriteBatch.Draw(tileSet, Right, tileSource, Color.Red);
    }

}
