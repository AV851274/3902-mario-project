using System.Collections.Generic;
using Mario.Game.Entities.Blocks;
using Mario.Game.Entities.Items;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;

namespace Mario.Game.Levels;

internal sealed class BlockTileHandler : ILevelTileHandler
{
    private readonly ICollection<IBlock> blocks;
    private readonly BlockFactory blockFactory;

    public BlockTileHandler(ICollection<IBlock> blocks, BlockFactory blockFactory)
    {
        this.blocks = blocks;
        this.blockFactory = blockFactory;
    }

    public bool Place(string tile, Vector2 position)
    {
        switch (tile)
        {
            case "G":
                blocks.Add(
                    blockFactory.Create(BlockType.Ground, position));
                return true;
            case "B":
                blocks.Add(
                    blockFactory.Create(BlockType.Brick, position));
                return true;
            case "?":
                blocks.Add(
                    blockFactory.Create(BlockType.Question, position, ItemType.Coin));
                return true;
            case "?m":
                blocks.Add(
                    blockFactory.Create(BlockType.Question, position, ItemType.Mushroom));
                return true;
            case "?s":
                blocks.Add(
                    blockFactory.Create(BlockType.Question, position, ItemType.Star));
                return true;
            case "Bl":
                blocks.Add(
                    blockFactory.Create(BlockType.Block, position));
                return true;
            case "treeLeft":
                blocks.Add(
                    blockFactory.Create(BlockType.TreeLeft, position));
                return true;
            case "treeRight":
                blocks.Add(
                    blockFactory.Create(BlockType.TreeRight, position));
                return true;
            case "treeMiddle":
                blocks.Add(
                    blockFactory.Create(BlockType.TreeMiddle, position));
                return true;
            case "treeRoot":
                blocks.Add(
                    blockFactory.Create(BlockType.TreeRoot, position));
                return true;
            case "launcherNeck":
                blocks.Add(
                    blockFactory.Create(BlockType.LauncherNeck, position));
                return true;
            case "launcherBody":
                blocks.Add(
                    blockFactory.Create(BlockType.LauncherBody, position));
                return true;
            case "PL":
                blocks.Add(blockFactory.Create(BlockType.PipeTopLeft, position));
                return true;
            case "PR":
                blocks.Add(blockFactory.Create(BlockType.PipeTopRight, position));
                return true;
            case "pl":
                blocks.Add(blockFactory.Create(BlockType.PipeLeft, position));
                return true;
            case "pr":
                blocks.Add(blockFactory.Create(BlockType.PipeRight, position));
                return true;
            case "warpL":
                blocks.Add(blockFactory.Create(BlockType.WarpPipeTopLeft, position));
                return true;
            case "warpR":
                blocks.Add(blockFactory.Create(BlockType.WarpPipeTopRight, position));
                return true;
            case "bridge":
                blocks.Add(blockFactory.Create(BlockType.Bridge, position));
                return true;
            case "cloud":
                blocks.Add(blockFactory.Create(BlockType.Cloud, position));
                return true;
            case "flagPole":
            case "F":
                blocks.Add(blockFactory.Create(BlockType.FlagPole, position));
                return true;
            case "/":
                blocks.Add(blockFactory.Create(BlockType.Wave, new Vector2(position.X, position.Y + 24)));
                return true;
            case "waterRock":
                blocks.Add(blockFactory.Create(BlockType.WaterRock, position));
                return true;
            case "coralBlock":
                blocks.Add(blockFactory.Create(BlockType.CoralBlock, position));
                return true;
            default:
                return false;
        }
    }
}
