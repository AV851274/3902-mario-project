using System;
using Mario.Game.Entities.Items;
using Mario.Game.Interfaces;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;

namespace Mario.Game.Entities.Blocks;

public sealed class BlockFactory
{
    private readonly SpriteFactory sprites;
    private readonly ItemFactory items;
    private readonly IEntitySpawner spawner;
    private readonly IGameAudio audio;

    public BlockFactory(SpriteFactory sprites, ItemFactory items, IEntitySpawner spawner, IGameAudio audio)
    {
        this.sprites = sprites;
        this.items = items;
        this.spawner = spawner;
        this.audio = audio;
    }

    public IBlock Create(BlockType type, Vector2 position, ItemType? contents = null)
    {
        switch (type)
        {
            case BlockType.Brick:
                return new BrickBlock(sprites.Create("blocks", type), position, audio);
            case BlockType.Question:
                return new QuestionBlock(
                    sprites.Create("blocks", type), sprites.Create("blocks", BlockType.UsedQuestion),
                    position, contents, items, spawner, audio);
            case BlockType.Cloud:
                return new CloudBlock(sprites.Create("blocks", type), position);
            case BlockType.Bridge:
            case BlockType.MovingPlatform:
                return new PlatformBlock(sprites.Create("blocks", type), type, position);
            case BlockType.WarpPipeTopLeft:
            case BlockType.WarpPipeTopRight:
                return new WarpPipeBlock(sprites.Create("blocks", type), type, position);
            case BlockType.FlagPole:
                return new FlagPoleBlock(sprites.Create("blocks", type), position);
            case BlockType.Block:
            case BlockType.UsedQuestion:
            case BlockType.Ground:
            case BlockType.BlueBrick:
            case BlockType.GreyBlock:
            case BlockType.PipeTopLeft:
            case BlockType.PipeTopRight:
            case BlockType.PipeLeft:
            case BlockType.PipeRight:
            case BlockType.TreeLeft:
            case BlockType.TreeRight:
            case BlockType.TreeMiddle:
            case BlockType.TreeRoot:
            case BlockType.LauncherHead:
            case BlockType.LauncherNeck:
            case BlockType.LauncherBody:
            case BlockType.WaterRock:
            case BlockType.CoralBlock:
            case BlockType.Wave:
                return new SolidBlock(sprites.Create("blocks", type), type, position);
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown block type");
        }
    }
}