using Mario.Game.Entities.Background;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mario.Game.Rendering.Sprites;

public class BackgroundSpriteFactory
{
    private readonly Texture2D tileSet;

    public BackgroundSpriteFactory(ContentManager content)
    {
        tileSet = content.Load<Texture2D>("tileset");
    }

    public ISprite CreateBackgroundSprite(BackgroundType type)
    {
        return type switch
        {
            BackgroundType.Cloud => new StaticSprite(tileSet, new Rectangle(624, 600, 48, 32)),
            BackgroundType.SmallCloud => new StaticSprite(tileSet, new Rectangle(352, 624, 32, 32)),
            BackgroundType.BigCloud => new StaticSprite(tileSet, new Rectangle(344, 403, 64, 32)),
            BackgroundType.BigHill => new StaticSprite(tileSet, new Rectangle(112, 728, 80, 48)),
            BackgroundType.SmallHill => new StaticSprite(tileSet, new Rectangle(200, 744, 48, 32)),
            BackgroundType.SmallBush => new StaticSprite(tileSet, new Rectangle(208, 704, 32, 16)),
            BackgroundType.BigBush => new StaticSprite(tileSet, new Rectangle(120, 704, 64, 16)),
            BackgroundType.BigCastle => new StaticSprite(tileSet, new Rectangle(400, 600, 144, 176)),
            BackgroundType.SmallCastle => new StaticSprite(tileSet, new Rectangle(24, 696, 80, 80)),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
