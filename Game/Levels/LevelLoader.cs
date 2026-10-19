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

    public static (Vector2 spawn, int width, int height) LoadLevel(
        ContentManager content,
        string fileName,
        GameEntities gameEntities,
        SpriteFactory spriteFactory)
    {
        using var csvin = new StreamReader(
            TitleContainer.OpenStream(Path.Combine(content.RootDirectory, "levels", fileName)));
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
                        gameEntities.Blocks.Add(
                            new Block(spriteFactory, BlockType.Ground, position));
                        break;

                    case "B":
                        gameEntities.Blocks.Add(
                            new Block(spriteFactory, BlockType.Brick, position));
                        break;

                    case "?":
                        gameEntities.Blocks.Add(
                            new Block(spriteFactory, BlockType.Question, position));
                        break;
                    case "Bl":
                        gameEntities.Blocks.Add(
                            new Block(spriteFactory, BlockType.Block, position));
                        break;
                    case "smCastle":
                        gameEntities.Background.Add(
                            new BackgroundObject(spriteFactory, BackgroundType.SmallCastle, position));
                        break;
                    case "bgCastle":
                        gameEntities.Background.Add(
                            new BackgroundObject(spriteFactory, BackgroundType.BigCastle, position));
                        break;
                    case "M":
                        marioSpawn = position;
                        break;
                    case "E":
                        gameEntities.Enemies.Add(
                            new Goomba(
                                spriteFactory.CreateGoombaSprite(),
                                position));
                        break;
                    case "K":
                        gameEntities.Enemies.Add(
                            new Turtle(
                                spriteFactory.CreateTurtleSprite(),
                                position));
                        break;
                    case "H":
                        gameEntities.Enemies.Add(
                            new HammerBro(
                                spriteFactory.CreateHammerBroSprite(),
                                position));
                        break;
                    case ".":
                    case "":
                        break;
                    case "mu":
                        gameEntities.Items.Add(new Item(spriteFactory, ItemType.Mushroom, position));
                        break;
                    case "fi":
                        gameEntities.Items.Add(new Item(spriteFactory, ItemType.FireFlower, position));
                        break;
                    case "co":
                        gameEntities.Items.Add(new Item(spriteFactory, ItemType.Coin, position));
                        break;
                    case "st":
                        gameEntities.Items.Add(new Item(spriteFactory, ItemType.Star, position));
                        break;
                    case "cl":
                        gameEntities.Background.Add(new BackgroundObject(spriteFactory, BackgroundType.Cloud, position));
                        break;
                    case "bh":
                        gameEntities.Background.Add(new BackgroundObject(spriteFactory, BackgroundType.BigHill, position));
                        break;
                    case "sh":
                        gameEntities.Background.Add(
                            new BackgroundObject(spriteFactory, BackgroundType.SmallHill, position));
                        break;
                    case "sb":
                        gameEntities.Background.Add(
                            new BackgroundObject(spriteFactory, BackgroundType.SmallBush, position));
                        break;
                    case "bb":
                        gameEntities.Background.Add(new BackgroundObject(spriteFactory, BackgroundType.BigBush, position));
                        break;
                    case "PL":
                        gameEntities.Blocks.Add(new Block(spriteFactory, BlockType.PipeTopLeft, position));
                        break;
                    case "PR":
                        gameEntities.Blocks.Add(new Block(spriteFactory, BlockType.PipeTopRight, position));
                        break;
                    case "pl":
                        gameEntities.Blocks.Add(new Block(spriteFactory, BlockType.PipeLeft, position));
                        break;
                    case "pr":
                        gameEntities.Blocks.Add(new Block(spriteFactory, BlockType.PipeRight, position));
                        break;

                    default:
                        throw new FormatException(
                            $"Unknown tile '{tile}' at row {row + 1}, column {col + 1}");
                }
            }
        }

        return (marioSpawn, levelWidth, rows.Count * TileSize);
    }
}