using System.Collections.Generic;
using Mario.Game.Entities.Blocks;
using Mario.Game.Entities.Enemies;
using Mario.Game.Interfaces;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;

namespace Mario.Game.Levels;

internal sealed class CompoundTileHandler : ILevelTileHandler
{
    private readonly ICollection<IBlock> blocks;
    private readonly ICollection<IEnemy> enemies;
    private readonly SpriteFactory spriteFactory;
    private readonly IEntitySpawner spawner;
    private readonly BlockFactory blockFactory;

    public CompoundTileHandler(ICollection<IBlock> blocks, ICollection<IEnemy> enemies,
        SpriteFactory spriteFactory, IEntitySpawner spawner, BlockFactory blockFactory)
    {
        this.blocks = blocks;
        this.enemies = enemies;
        this.spriteFactory = spriteFactory;
        this.spawner = spawner;
        this.blockFactory = blockFactory;
    }

    public bool Place(string tile, Vector2 position)
    {
        switch (tile)
        {
            case "Fb":
                enemies.Add(
                    new FireBar(
                        spriteFactory.Create("projectiles", "fireball"),
                        new Vector2(position.X + 12, position.Y + 12), 6));

                blocks.Add(
                    blockFactory.Create(BlockType.UsedQuestion, position));
                return true;
            case "Bbl":
                enemies.Add(
                    new BillLauncher(
                        null,
                        position, spawner));
                blocks.Add(
                    blockFactory.Create(BlockType.LauncherHead, position));
                return true;
            default:
                return false;
        }
    }
}
