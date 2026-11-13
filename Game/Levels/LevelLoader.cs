using System;
using Mario.Game.Interfaces;
using Mario.Game.Rendering.Sprites;
using Mario.Game.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Mario.Game.Levels;

public class LevelLoader
{
    private const int TileSize = 48;

    public static (Vector2 spawn, int width, int height) LoadLevel(
        ContentManager content, string fileName, GameEntities gameEntities,
        SpriteFactory spriteFactory, IEntitySpawner spawner)
    {
        var rows = CsvLevelReader.Read(content, fileName);

        ILevelTileHandler[] handlers =
        [
            new BlockTileHandler(gameEntities.Blocks, spriteFactory, spawner),
            new EnemyTileHandler(gameEntities.Enemies, spriteFactory, spawner),
            new ItemTileHandler(gameEntities.Items, spriteFactory),
            new BackgroundTileHandler(gameEntities.Background, spriteFactory),
            new CompoundTileHandler(gameEntities.Blocks, gameEntities.Enemies, spriteFactory, spawner)
        ];

        Vector2 marioSpawn = new(100, 100);
        int levelWidth = 0;

        for (int row = 0; row < rows.Count; row++)
        {
            string[] tiles = rows[row];
            levelWidth = Math.Max(levelWidth, tiles.Length * TileSize);

            for (int col = 0; col < tiles.Length; col++)
            {
                string tile = tiles[col].Trim();
                Vector2 position = new(col * TileSize, row * TileSize);

                if (tile is "" or ".") continue;
                if (tile == "M")
                {
                    marioSpawn = position;
                    continue;
                }

                bool handled = false;
                foreach (var handler in handlers)
                {
                    if (!handler.Place(tile, position)) continue;
                    handled = true;
                    break;
                }

                if (!handled)
                    throw new FormatException(
                        $"Unknown tile '{tile}' at row {row + 1}, column {col + 1}");
            }
        }

        return (marioSpawn, levelWidth, rows.Count * TileSize);
    }
}
