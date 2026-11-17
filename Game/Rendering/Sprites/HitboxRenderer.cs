using Microsoft.Xna.Framework;
using Mario.Game.Assets;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Rendering.Sprites;

public class HitboxRenderer
{
    private const int BorderThickness = 1;
    private readonly Texture2D tileSet;
    private readonly Rectangle tileSource;
    public HitboxRenderer(AssetManager assets)
    {
        var definition = assets.GetSpriteDefinition("debug", "hitbox");
        tileSet = assets.GetTexture(definition.Texture);
        tileSource = definition.Rect;
    }

    public void DrawHitbox(SpriteBatch spriteBatch, float left, float right, float top, float bottom)
    {
        Rectangle Top = new Rectangle(
            (int)left, (int)top, (int)(right - left), BorderThickness);
        Rectangle Bottom = new Rectangle(
            (int)left, (int)bottom - BorderThickness, (int)(right - left), BorderThickness);
        Rectangle Left = new Rectangle(
            (int)left, (int)top, BorderThickness, (int)(bottom - top));
        Rectangle Right = new Rectangle(
            (int)right - BorderThickness, (int)top, BorderThickness, (int)(bottom - top));

        spriteBatch.Draw(tileSet, Top, tileSource, Color.Red);
        spriteBatch.Draw(tileSet, Bottom, tileSource, Color.Red);
        spriteBatch.Draw(tileSet, Left, tileSource, Color.Red);
        spriteBatch.Draw(tileSet, Right, tileSource, Color.Red);
    }

}
