using System.Collections.Generic;
using Mario.Game.Entities.Items;
using Mario.Game.Interfaces;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;

namespace Mario.Game.Levels;

internal sealed class ItemTileHandler : ILevelTileHandler
{
    private readonly ICollection<IItem> items;
    private readonly SpriteFactory spriteFactory;

    public ItemTileHandler(ICollection<IItem> items, SpriteFactory spriteFactory)
    {
        this.items = items;
        this.spriteFactory = spriteFactory;
    }

    public bool Place(string tile, Vector2 position)
    {
        switch (tile)
        {
            case "mu":
                items.Add(new Item(spriteFactory, ItemType.Mushroom, position));
                return true;
            case "fi":
                items.Add(new Item(spriteFactory, ItemType.FireFlower, position));
                return true;
            case "co":
                items.Add(new Item(spriteFactory, ItemType.Coin, new Vector2(position.X + 12, position.Y)));
                return true;
            case "st":
                items.Add(new Item(spriteFactory, ItemType.Star, position));
                return true;
            default:
                return false;
        }
    }
}
