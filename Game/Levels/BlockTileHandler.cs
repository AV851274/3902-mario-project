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

    public BlockTileHandler(ICollection<IBlock> blocks, SpriteFactory spriteFactory, IEntitySpawner spawner)
    {
        this.blocks = blocks;
        this.spriteFactory = spriteFactory;
        this.spawner = spawner;
    }

    public bool Place(string tile, Vector2 position)
    {
        switch (tile)
        {
            case "G":
                blocks.Add(
                    new Block(spriteFactory, spawner, BlockType.Ground, position));
                return true;
            case "B":
                blocks.Add(
                    new Block(spriteFactory, spawner, BlockType.Brick, position));
                return true;
            case "?":
                blocks.Add(
                    new Block(spriteFactory, spawner, BlockType.Question, position, ItemType.Coin));
                return true;
            case "?m":
                blocks.Add(
                    new Block(spriteFactory, spawner, BlockType.Question, position, ItemType.Mushroom));
                return true;
            case "?s":
                blocks.Add(
                    new Block(spriteFactory, spawner, BlockType.Question, position, ItemType.Star));
                return true;
            case "Bl":
                blocks.Add(
                    new Block(spriteFactory, spawner, BlockType.Block, position));
                return true;
            case "treeLeft":
                blocks.Add(
                    new Block(spriteFactory, spawner, BlockType.TreeLeft, position));
                return true;
            case "treeRight":
                blocks.Add(
                    new Block(spriteFactory, spawner, BlockType.TreeRight, position));
                return true;
            case "treeMiddle":
                blocks.Add(
                    new Block(spriteFactory, spawner, BlockType.TreeMiddle, position));
                return true;
            case "treeRoot":
                blocks.Add(
                    new Block(spriteFactory, spawner, BlockType.TreeRoot, position));
                return true;
            case "launcherNeck":
                blocks.Add(
                    new Block(spriteFactory, spawner, BlockType.LauncherNeck, position));
                return true;
            case "launcherBody":
                blocks.Add(
                    new Block(spriteFactory, spawner, BlockType.LauncherBody, position));
                return true;
            case "PL":
                blocks.Add(new Block(spriteFactory, spawner, BlockType.PipeTopLeft, position));
                return true;
            case "PR":
                blocks.Add(new Block(spriteFactory, spawner, BlockType.PipeTopRight, position));
                return true;
            case "pl":
                blocks.Add(new Block(spriteFactory, spawner, BlockType.PipeLeft, position));
                return true;
            case "pr":
                blocks.Add(new Block(spriteFactory, spawner, BlockType.PipeRight, position));
                return true;
            case "warpL":
                blocks.Add(new Block(spriteFactory, spawner, BlockType.WarpPipeTopLeft, position));
                return true;
            case "warpR":
                blocks.Add(new Block(spriteFactory, spawner, BlockType.WarpPipeTopRight, position));
                return true;
            case "bridge":
                blocks.Add(new Block(spriteFactory, spawner, BlockType.Bridge, position));
                return true;
            case "cloud":
                blocks.Add(new Block(spriteFactory, spawner, BlockType.Cloud, position));
                return true;
            case "flagPole":
            case "F":
                blocks.Add(new Block(spriteFactory, spawner, BlockType.FlagPole, position));
                return true;
            case "/":
                blocks.Add(new Block(spriteFactory, spawner, BlockType.Wave, new Vector2(position.X, position.Y + 24)));
                return true;
            case "waterRock":
                blocks.Add(new Block(spriteFactory, spawner, BlockType.WaterRock, position));
                return true;
            case "coralBlock":
                blocks.Add(new Block(spriteFactory, spawner, BlockType.CoralBlock, position));
                return true;
            default:
                return false;
        }
    }
}
