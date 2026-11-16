using System.Collections.Generic;
using Mario.Game.Entities.Background;
using Mario.Game.Interfaces;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;

namespace Mario.Game.Levels;

internal sealed class BackgroundTileHandler : ILevelTileHandler
{
    private readonly ICollection<IGameObject> background;
    private readonly SpriteFactory spriteFactory;

    public BackgroundTileHandler(ICollection<IGameObject> background, SpriteFactory spriteFactory)
    {
        this.background = background;
        this.spriteFactory = spriteFactory;
    }

    public bool Place(string tile, Vector2 position)
    {
        switch (tile)
        {
            case "smCastle":
                background.Add(
                    new BackgroundObject(spriteFactory, BackgroundType.SmallCastle, position));
                return true;
            case "bgCastle":
                background.Add(
                    new BackgroundObject(spriteFactory, BackgroundType.BigCastle, position));
                return true;
            case "cl":
                background.Add(new BackgroundObject(spriteFactory, BackgroundType.Cloud, new Vector2(position.X + 24, position.Y - 24)));
                return true;
            case "smcl":
                background.Add(new BackgroundObject(spriteFactory, BackgroundType.SmallCloud, new Vector2(position.X + 24, position.Y - 24)));
                return true;
            case "bcl":
                background.Add(new BackgroundObject(spriteFactory, BackgroundType.BigCloud, new Vector2(position.X + 24, position.Y - 24)));
                return true;
            case "bh":
                background.Add(new BackgroundObject(spriteFactory, BackgroundType.BigHill, new Vector2(position.X, position.Y + 48)));
                return true;
            case "sh":
                background.Add(
                    new BackgroundObject(spriteFactory, BackgroundType.SmallHill, new Vector2(position.X, position.Y - 48)));
                return true;
            case "sb":
                background.Add(
                    new BackgroundObject(spriteFactory, BackgroundType.SmallBush, new Vector2(position.X + 24, position.Y)));
                return true;
            case "bb":
                background.Add(new BackgroundObject(spriteFactory, BackgroundType.BigBush, new Vector2(position.X + 24, position.Y)));
                return true;
            case "w":
                background.Add(new BackgroundObject(spriteFactory, BackgroundType.Water, new Vector2(position.X, position.Y + 24)));
                return true;
            case "flag":
            case "fl":
                background.Add(new BackgroundObject(spriteFactory, BackgroundType.Flag, position));
                return true;
            default:
                return false;
        }
    }
}
