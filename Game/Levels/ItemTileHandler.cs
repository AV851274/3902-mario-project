using System.Collections.Generic;
using Mario.Game.Entities.Items;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;

namespace Mario.Game.Levels;

internal sealed class ItemTileHandler : ILevelTileHandler
{
    private readonly ICollection<IItem> items;
    private readonly ItemFactory itemFactory;

    public ItemTileHandler(ICollection<IItem> items, ItemFactory itemFactory)
    {
        this.items = items;
        this.itemFactory = itemFactory;
    }

    public bool Place(string tile, Vector2 position)
    {
        switch (tile)
        {
            case "mu":
                items.Add(itemFactory.Create(ItemType.Mushroom, position));
                return true;
            case "fi":
                items.Add(itemFactory.Create(ItemType.FireFlower, position));
                return true;
            case "co":
                items.Add(itemFactory.Create(ItemType.Coin,
                    new Vector2(position.X + GameConstants.World.QuarterTileSize, position.Y)));
                return true;
            case "st":
                items.Add(itemFactory.Create(ItemType.Star, position));
                return true;
            default:
                return false;
        }
    }
}
