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
using static Mario.Game.Entities.Enemies.EnemyBase;

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
                            new Block(spriteFactory.Blocks, BlockType.Ground, position));
                        break;

                    case "B":
                        gameEntities.Blocks.Add(
                            new Block(spriteFactory.Blocks, BlockType.Brick, position));
                        break;

                    case "?":
                        gameEntities.Blocks.Add(
                            new Block(spriteFactory.Blocks, BlockType.Question, position, ItemType.Coin));
                        break;
                    case "?m":
                        gameEntities.Blocks.Add(
                            new Block(spriteFactory.Blocks, BlockType.Question, position, ItemType.Mushroom));
                        break;
                    case "?s":
                        gameEntities.Blocks.Add(
                            new Block(spriteFactory.Blocks, BlockType.Question, position, ItemType.Star));
                        break;
                    case "Bl":
                        gameEntities.Blocks.Add(
                            new Block(spriteFactory.Blocks, BlockType.Block, position));
                        break;
                    case "treeLeft":
                        gameEntities.Blocks.Add(
                            new Block(spriteFactory.Blocks, BlockType.TreeLeft, position));
                        break;
                    case "treeRight":
                        gameEntities.Blocks.Add(
                            new Block(spriteFactory.Blocks, BlockType.TreeRight, position));
                        break;
                    case "treeMiddle":
                        gameEntities.Blocks.Add(
                            new Block(spriteFactory.Blocks, BlockType.TreeMiddle, position));
                        break;
                    case "treeRoot":
                        gameEntities.Blocks.Add(
                            new Block(spriteFactory.Blocks, BlockType.TreeRoot, position));
                        break;
                    case "smCastle":
                        gameEntities.Background.Add(
                            new BackgroundObject(spriteFactory.Background, BackgroundType.SmallCastle, position));
                        break;
                    case "bgCastle":
                        gameEntities.Background.Add(
                            new BackgroundObject(spriteFactory.Background, BackgroundType.BigCastle, position));
                        break;
                    case "M":
                        marioSpawn = position;
                        break;
                    case "E":
                        gameEntities.Enemies.Add(
                            new Goomba(
                                spriteFactory.Enemies.CreateGoombaSprite(),
                                position));
                        break;
                    case "K":
                        gameEntities.Enemies.Add(
                            new Turtle(
                                spriteFactory.Enemies.CreateTurtleSprite(),
                                position));
                        break;
                    case "H":
                        gameEntities.Enemies.Add(
                            new HammerBro(
                                spriteFactory.Enemies.CreateHammerBroSprite(),
                                position));
                        break;
                    case "Bw":
                        gameEntities.Enemies.Add(
                            new Bowser(
                                spriteFactory.Enemies.CreateBowserSprite(),
                                position));
                        break;
                    case "Lk":
                        gameEntities.Enemies.Add(
                            new Lakitu(
                                spriteFactory.Enemies.CreateLakituSprite(),
                                position));
                        break;
                    case "Bz":
                        gameEntities.Enemies.Add(
                            new BuzzyBeetle(
                                spriteFactory.Enemies.CreateBuzzyBeetleSprite(),
                                position));
                        break;
                    case "Fb":
                        gameEntities.Enemies.Add(
                            new FireBar(
                                spriteFactory.Mario.CreateFireballSprite(),
                                new Vector2(position.X + 12, position.Y + 12), 6));

                        gameEntities.Blocks.Add(
                            new Block(spriteFactory.Blocks, 
                                BlockType.UsedQuestion, 
                                position));

                        break;
                    case "Sp":
                        gameEntities.Enemies.Add(
                            new Spiny(
                                spriteFactory.Enemies.CreateSpinySprite(),
                                position));
                        break;
                    case "Pk":
                        gameEntities.Enemies.Add(
                            new ParaKoopa(
                                spriteFactory.Enemies.CreateParaKoopaSprite(),
                                position));
                        break;

                    case "GrayCheep":
                        gameEntities.Enemies.Add(
                            new CheepCheep(
                                spriteFactory.Enemies.CreateGrayCheepSprite(),
                                position,
                                FishColor.Gray));
                        break;
                    case "GreenCheep":
                        gameEntities.Enemies.Add(
                            new CheepCheep(
                                spriteFactory.Enemies.CreateGreenCheepSprite(),
                                position,
                                FishColor.Green));
                        break;
                    case ".":
                    case "":
                        break;
                    case "mu":
                        gameEntities.Items.Add(new Item(spriteFactory.Items, ItemType.Mushroom, position));
                        break;
                    case "fi":
                        gameEntities.Items.Add(new Item(spriteFactory.Items, ItemType.FireFlower, position));
                        break;
                    case "co":
                        gameEntities.Items.Add(new Item(spriteFactory.Items, ItemType.Coin, new Vector2(position.X + 12, position.Y)));
                        break;
                    case "st":
                        gameEntities.Items.Add(new Item(spriteFactory.Items, ItemType.Star, position));
                        break;
                    case "cl":
                        gameEntities.Background.Add(new BackgroundObject(spriteFactory.Background, BackgroundType.Cloud, new Vector2(position.X + 24, position.Y - 24)));
                        break;
                    case "smcl":
                        gameEntities.Background.Add(new BackgroundObject(spriteFactory.Background, BackgroundType.SmallCloud, new Vector2(position.X + 24, position.Y - 24)));
                        break;
                    case "bcl":
                        gameEntities.Background.Add(new BackgroundObject(spriteFactory.Background, BackgroundType.BigCloud, new Vector2(position.X + 24, position.Y - 24)));
                        break;
                    case "bh":
                        gameEntities.Background.Add(new BackgroundObject(spriteFactory.Background, BackgroundType.BigHill, new Vector2(position.X, position.Y + 48)));
                        break;
                    case "sh":
                        gameEntities.Background.Add(
                            new BackgroundObject(spriteFactory.Background, BackgroundType.SmallHill, new Vector2(position.X, position.Y - 48)));
                        break;
                    case "sb":
                        gameEntities.Background.Add(
                            new BackgroundObject(spriteFactory.Background, BackgroundType.SmallBush, new Vector2(position.X + 24, position.Y)));
                        break;
                    case "bb":
                        gameEntities.Background.Add(new BackgroundObject(spriteFactory.Background, BackgroundType.BigBush, new Vector2(position.X + 24, position.Y)));
                        break;
                    case "PL":
                        gameEntities.Blocks.Add(new Block(spriteFactory.Blocks, BlockType.PipeTopLeft, position));
                        break;
                    case "PR":
                        gameEntities.Blocks.Add(new Block(spriteFactory.Blocks, BlockType.PipeTopRight, position));
                        break;
                    case "pl":
                        gameEntities.Blocks.Add(new Block(spriteFactory.Blocks, BlockType.PipeLeft, position));
                        break;
                    case "pr":
                        gameEntities.Blocks.Add(new Block(spriteFactory.Blocks, BlockType.PipeRight, position));
                        break;
                    case "bridge":
                        gameEntities.Blocks.Add(new Block(spriteFactory.Blocks, BlockType.Bridge, position));
                        break;
                    case "cloud":
                        gameEntities.Blocks.Add(new Block(spriteFactory.Blocks, BlockType.Cloud, position));
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