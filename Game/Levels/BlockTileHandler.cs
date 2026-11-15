using System.Collections.Generic;
using Mario.Game.Entities.Blocks;
using Mario.Game.Entities.Items;
using Mario.Game.Interfaces;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;

namespace Mario.Game.Levels;

internal sealed class BlockTileHandler : ILevelTileHandler
{
    private readonly ICollection<IBlock> blocks;
    private readonly SpriteFactory spriteFactory;
    private readonly IEntitySpawner spawner;
    private readonly ItemFactory itemFactory;
    private readonly IGameAudio audio;

    public BlockTileHandler(ICollection<IBlock> blocks, SpriteFactory spriteFactory,
        IEntitySpawner spawner, ItemFactory itemFactory, IGameAudio audio)
    {
        this.blocks = blocks;
        this.spriteFactory = spriteFactory;
        this.spawner = spawner;
        this.itemFactory = itemFactory;
        this.audio = audio;
    }

    public bool Place(string tile, Vector2 position)
    {
        switch (tile)
        {
            case "G":
                blocks.Add(
                    new Block(spriteFactory, spawner, itemFactory, audio, BlockType.Ground, position));
                return true;
            case "B":
                blocks.Add(
                    new Block(spriteFactory, spawner, itemFactory, audio, BlockType.Brick, position));
                return true;
            case "?":
                blocks.Add(
                    new Block(spriteFactory, spawner, itemFactory, audio, BlockType.Question, position, ItemType.Coin));
                return true;
            case "?m":
                blocks.Add(
                    new Block(spriteFactory, spawner, itemFactory, audio, BlockType.Question, position, ItemType.Mushroom));
                return true;
            case "?s":
                blocks.Add(
                    new Block(spriteFactory, spawner, itemFactory, audio, BlockType.Question, position, ItemType.Star));
                return true;
            case "Bl":
                blocks.Add(
                    new Block(spriteFactory, spawner, itemFactory, audio, BlockType.Block, position));
                return true;
            case "treeLeft":
                blocks.Add(
                    new Block(spriteFactory, spawner, itemFactory, audio, BlockType.TreeLeft, position));
                return true;
            case "treeRight":
                blocks.Add(
                    new Block(spriteFactory, spawner, itemFactory, audio, BlockType.TreeRight, position));
                return true;
            case "treeMiddle":
                blocks.Add(
                    new Block(spriteFactory, spawner, itemFactory, audio, BlockType.TreeMiddle, position));
                return true;
            case "treeRoot":
                blocks.Add(
                    new Block(spriteFactory, spawner, itemFactory, audio, BlockType.TreeRoot, position));
                return true;
            case "launcherNeck":
                blocks.Add(
                    new Block(spriteFactory, spawner, itemFactory, audio, BlockType.LauncherNeck, position));
                return true;
            case "launcherBody":
                blocks.Add(
                    new Block(spriteFactory, spawner, itemFactory, audio, BlockType.LauncherBody, position));
                return true;
            case "PL":
                blocks.Add(new Block(spriteFactory, spawner, itemFactory, audio, BlockType.PipeTopLeft, position));
                return true;
            case "PR":
                blocks.Add(new Block(spriteFactory, spawner, itemFactory, audio, BlockType.PipeTopRight, position));
                return true;
            case "pl":
                blocks.Add(new Block(spriteFactory, spawner, itemFactory, audio, BlockType.PipeLeft, position));
                return true;
            case "pr":
                blocks.Add(new Block(spriteFactory, spawner, itemFactory, audio, BlockType.PipeRight, position));
                return true;
            case "warpL":
                blocks.Add(new Block(spriteFactory, spawner, itemFactory, audio, BlockType.WarpPipeTopLeft, position));
                return true;
            case "warpR":
                blocks.Add(new Block(spriteFactory, spawner, itemFactory, audio, BlockType.WarpPipeTopRight, position));
                return true;
            case "bridge":
                blocks.Add(new Block(spriteFactory, spawner, itemFactory, audio, BlockType.Bridge, position));
                return true;
            case "cloud":
                blocks.Add(new Block(spriteFactory, spawner, itemFactory, audio, BlockType.Cloud, position));
                return true;
            case "flagPole":
            case "F":
                blocks.Add(new Block(spriteFactory, spawner, itemFactory, audio, BlockType.FlagPole, position));
                return true;
            case "/":
                blocks.Add(new Block(spriteFactory, spawner, itemFactory, audio, BlockType.Wave, new Vector2(position.X, position.Y + 24)));
                return true;
            case "waterRock":
                blocks.Add(new Block(spriteFactory, spawner, itemFactory, audio, BlockType.WaterRock, position));
                return true;
            case "coralBlock":
                blocks.Add(new Block(spriteFactory, spawner, itemFactory, audio, BlockType.CoralBlock, position));
                return true;
            default:
                return false;
        }
    }
}
