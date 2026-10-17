using System;
using System.Collections.Generic;
using System.IO;
using Mario.Game.Entities.Blocks;
using Mario.Game.State;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;
using Mario.Game.Entities.Enemies;
using Mario.Game.Entities.Items;
using Mario.Game.Entities.Background;
using Microsoft.Xna.Framework.Content;

namespace Mario.Game.Levels;

public class LevelLoader
{
    private const int TileSize = 48;

    public static (Vector2 spawn, int width) LoadLevel(
        ContentManager content,
        GameState gameState,
        SpriteFactory spriteFactory)
    {
        using var csvin = new StreamReader(
            TitleContainer.OpenStream(Path.Combine(content.RootDirectory, "levels", "level1.csv")));
        List<String> rows = [];
        while (!csvin.EndOfStream)
        {
            rows.Add(csvin.ReadLine());
        }

        Vector2 marioSpawn = new Vector2(100, 100);
        int levelWidth = 0;

        for (int row = 0; row < rows.Count; row++)
        {
            string[] tiles = rows[row].Split(',');
            levelWidth = Math.Max(levelWidth, tiles.Length * TileSize);

            for (int col = 0; col < tiles.Length; col++)
            {
                string tile = tiles[col].Trim();

                Vector2 position = new Vector2(
                    col * TileSize,
                    row * TileSize);

                switch (tile)
                {
                    case "G":
                        gameState.Blocks.Add(
                            new Block(spriteFactory, BlockType.Ground, position));
                        break;

                    case "B":
                        gameState.Blocks.Add(
                            new Block(spriteFactory, BlockType.Brick, position));
                        break;

                    case "?":
                        gameState.Blocks.Add(
                            new Block(spriteFactory, BlockType.Question, position));
                        break;

                    case "M":
                        marioSpawn = position;
                        break;
                    case "E":
                        gameState.Enemies.Add(
                            new Goomba(
                                spriteFactory.CreateGoombaSprite(),
                                position));
                        break;
                    case "K":
                        gameState.Enemies.Add(
                            new Turtle(
                                spriteFactory.CreateTurtleSprite(),
                                position));
                        break;
                    case "H":
                        gameState.Enemies.Add(
                            new HammerBro(
                                spriteFactory.CreateHammerBroSprite(),
                                position));
                        break;
                    case ".":
                    case "":
                        break;
                    case "mu":
                        gameState.Items.Add(new Item(spriteFactory, ItemType.Mushroom, position));
                        break;
                    case "fi":
                        gameState.Items.Add(new Item(spriteFactory, ItemType.FireFlower, position));
                        break;
                    case "co":
                        gameState.Items.Add(new Item(spriteFactory, ItemType.Coin, position));
                        break;
                    case "st":
                        gameState.Items.Add(new Item(spriteFactory, ItemType.Star, position));
                        break;
                    case "cl":
                        gameState.Background.Add(new BackgroundObject(spriteFactory, BackgroundType.Cloud, position));
                        break;
                    case "bh":
                        gameState.Background.Add(new BackgroundObject(spriteFactory, BackgroundType.BigHill, position));
                        break;
                    case "sh":
                        gameState.Background.Add(
                            new BackgroundObject(spriteFactory, BackgroundType.SmallHill, position));
                        break;
                    case "sb":
                        gameState.Background.Add(
                            new BackgroundObject(spriteFactory, BackgroundType.SmallBush, position));
                        break;
                    case "bb":
                        gameState.Background.Add(new BackgroundObject(spriteFactory, BackgroundType.BigBush, position));
                        break;
                    case "PL":
                        gameState.Blocks.Add(new Block(spriteFactory, BlockType.PipeTopLeft, position));
                        break;
                    case "PR":
                        gameState.Blocks.Add(new Block(spriteFactory, BlockType.PipeTopRight, position));
                        break;
                    case "pl":
                        gameState.Blocks.Add(new Block(spriteFactory, BlockType.PipeLeft, position));
                        break;
                    case "pr":
                        gameState.Blocks.Add(new Block(spriteFactory, BlockType.PipeRight, position));
                        break;

                    default:
                        throw new FormatException(
                            $"Unknown tile '{tile}' at row {row + 1}, column {col + 1}");
                }
            }
        }

        return (marioSpawn, levelWidth);
    }
}