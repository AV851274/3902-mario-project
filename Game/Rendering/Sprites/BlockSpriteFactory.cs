using Mario.Game.Animation;
using Mario.Game.Entities.Blocks;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mario.Game.Rendering.Sprites;

public class BlockSpriteFactory
{
    private readonly Texture2D tileSet;
    private readonly Texture2D blockTexture;

    public BlockSpriteFactory(ContentManager content)
    {
        tileSet = content.Load<Texture2D>("tileset");
        blockTexture = content.Load<Texture2D>("itemsAndBlocks");
    }

    public ISprite CreateBlockSprite(BlockType type) //need to add Moving Platform
    {
        return type switch
        {
            BlockType.Brick => new StaticSprite(tileSet, new Rectangle(17, 16, 16, 16)),
            BlockType.Block => new StaticSprite(tileSet, new Rectangle(0, 33, 16, 16)),
            BlockType.Question => CreateQuestionBlockSprite(),
            BlockType.UsedQuestion => new StaticSprite(tileSet, new Rectangle(349, 78, 16, 16)),
            BlockType.Ground => new StaticSprite(tileSet, new Rectangle(0, 16, 16, 16)),
            BlockType.BlueBrick => new StaticSprite(blockTexture, new Rectangle(180, 116, 16, 16)),
            BlockType.GreyBlock => new StaticSprite(blockTexture, new Rectangle(180, 332, 16, 16)),
            BlockType.PipeTopLeft => new StaticSprite(tileSet, new Rectangle(119, 196, 16, 16)),
            BlockType.PipeTopRight => new StaticSprite(tileSet, new Rectangle(136, 196, 16, 16)),
            BlockType.PipeLeft => new StaticSprite(tileSet, new Rectangle(119, 213, 16, 16)),
            BlockType.PipeRight => new StaticSprite(tileSet, new Rectangle(136, 213, 16, 16)),
            BlockType.TreeRoot => new StaticSprite(tileSet, new Rectangle(628, 216, 16, 16)),
            BlockType.TreeLeft => new StaticSprite(tileSet, new Rectangle(611, 200, 16, 16)),
            BlockType.TreeRight => new StaticSprite(tileSet, new Rectangle(660, 200, 16, 16)),
            BlockType.TreeMiddle => new StaticSprite(tileSet, new Rectangle(628, 200, 16, 16)),
            BlockType.Bridge => new StaticSprite(tileSet, new Rectangle(558, 33, 16, 16)),
            BlockType.Cloud => new StaticSprite(tileSet, new Rectangle(366, 16, 16, 16)),
            BlockType.LauncherHead => new StaticSprite(tileSet, new Rectangle(119, 16, 16, 16)),
            BlockType.LauncherNeck => new StaticSprite(tileSet, new Rectangle(119, 33, 16, 16)),
            BlockType.LauncherBody => new StaticSprite(tileSet, new Rectangle(119, 50, 16, 16)),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }

    private AnimationController CreateQuestionBlockSprite()
    {
        AnimationController qBlock = new AnimationController(tileSet, Vector2.Zero, 3f);

        qBlock.AddClip(new Track(
            "Spin",
            [
                new Rectangle(298, 78, 16, 16),
                new Rectangle(315, 78, 16, 16),
                new Rectangle(332, 78, 16, 16),
            ],
            [0.2f, 0.2f, 0.2f],
            true));
        qBlock.Play("Spin");

        return qBlock;
    }
}
