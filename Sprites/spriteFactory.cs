using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Game2D.Animation;
using Game2D.Interfaces;

public class SpriteFactory
{
    private Texture2D marioTexture;
    private Texture2D itemTexture;
    private Texture2D blockTexture;

    public void LoadTextures(ContentManager content)
    {
        marioTexture = content.Load<Texture2D>("mario");
        itemTexture = content.Load<Texture2D>("itemsAndBlocks");
        blockTexture = content.Load<Texture2D>("itemsAndBlocks");
    }

    public List<ISprite> CreateItemSprites()
    {
        return new List<ISprite>
        {
            new StaticSprite(itemTexture, new Rectangle(0, 8, 16, 16)),    // mushroom
            new StaticSprite(itemTexture, new Rectangle(32, 8, 16, 16)),   // fire flower
            CreateCoinSprite(),
            CreateStarSprite(),
        };
    }

    private ISprite CreateCoinSprite()
    {
        AnimationController coin = new AnimationController(itemTexture, Vector2.Zero, 3f);

        coin.AddClip(new Track(
            "Spin",
            [
                new Rectangle(180, 36, 8, 15),
                new Rectangle(190, 36, 8, 15),
                new Rectangle(200, 36, 8, 15),
                new Rectangle(210, 36, 8, 15)
            ],
            [0.15f, 0.15f, 0.15f, 0.15f],
            true));
        coin.Play("Spin");

        return coin;
    }

    private ISprite CreateStarSprite()
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

    public List<ISprite> CreateBlockSprites()
    {
        return new List<ISprite>
        {
            new StaticSprite(blockTexture, new Rectangle(328, 128, 16, 16)),   // brick
            new StaticSprite(blockTexture, new Rectangle(180, 7, 16, 16)),  // question block
            new StaticSprite(blockTexture, new Rectangle(180, 116, 16, 16)),  // blue brick
            new StaticSprite(blockTexture, new Rectangle(180, 332, 16, 16)),  // grey block
        };
    }

    public ISprite CreatePlayerSprite()
    {
        AnimationController animationController = new AnimationController(
            marioTexture,
            Vector2.Zero,
            2.5f);

        animationController.AddClip(new Track(
            "Idle",
            [new Rectangle(0, 57, 16, 22)],
            [1f],
            true));
        animationController.AddClip(new Track(
            "Run",
            [
                new Rectangle(60, 0, 14, 16),
                new Rectangle(89, 0, 16, 16),
                new Rectangle(121, 0, 12, 16)
            ],
            [0.15f, 0.15f, 0.15f],
            true));
        animationController.AddClip(new Track(
            "Jump",
            [new Rectangle(150, 0, 14, 15)],
            [1f],
            false));
        animationController.Play("Idle");

        return animationController;
    }
}