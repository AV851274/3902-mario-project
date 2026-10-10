using System;
using System.IO;
using Mario.Blocks;
using Mario.GameStates;
using Mario.Sprites;
using Microsoft.Xna.Framework;
using Mario.Enemies;

namespace Mario.Levels;

public class LevelLoader
{
    private const int TileSize = 48;

    public static (Vector2 spawn, int width) LoadLevel(
    string filePath,
    GameState gameState,
    SpriteFactory spriteFactory)
    {
        string[] rows = File.ReadAllLines(filePath);
        Vector2 marioSpawn = new Vector2(100, 100);
        int levelWidth = 0;

        for (int row = 0; row < rows.Length; row++)
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
                    case ".":
                    case "":
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
