using Mario.Game.Animation;
using Mario.Game.Entities.Items;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mario.Game.Rendering.Sprites;

public class ItemSpriteFactory
{
    private readonly Texture2D itemTexture;

    public ItemSpriteFactory(ContentManager content)
    {
        itemTexture = content.Load<Texture2D>("itemsAndBlocks");
    }

    public ISprite CreateItemSprite(ItemType type)
    {
        return type switch
        {
            ItemType.Mushroom => new StaticSprite(itemTexture, new Rectangle(0, 8, 16, 16)),
            ItemType.FireFlower => new StaticSprite(itemTexture, new Rectangle(32, 8, 16, 16)),
            ItemType.Coin => CreateCoinSprite(),
            ItemType.Star => CreateStarSprite(),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }

    private AnimationController CreateCoinSprite()
    {
        AnimationController coin = new AnimationController(itemTexture, Vector2.Zero, 3f);

        coin.AddClip(new Track(
            "Spin",
            [
                new Rectangle(180, 35, 8, 16),
                new Rectangle(190, 35, 8, 16),
                new Rectangle(200, 35, 8, 16),
                new Rectangle(210, 35, 8, 16)
            ],
            [0.15f, 0.15f, 0.15f, 0.15f],
            true));
        coin.Play("Spin");

        return coin;
    }

    private AnimationController CreateStarSprite()
    {
        AnimationController star = new AnimationController(itemTexture, Vector2.Zero, 3f);

        star.AddClip(new Track(
            "Spin",
            [
                new Rectangle(142, 8, 16, 16),
                new Rectangle(160, 8, 16, 16),
                new Rectangle(142, 8, 16, 16),
                new Rectangle(160, 8, 16, 16),
            ],
            [0.15f, 0.15f, 0.15f, 0.15f],
            true));
        star.Play("Spin");

        return star;
    }
}
