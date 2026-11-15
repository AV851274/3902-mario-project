using System.Collections.Generic;
using Mario.Game.Entities.Blocks;
using Mario.Game.Entities.Enemies;
using Mario.Game.Entities.Items;
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
    private readonly ItemFactory itemFactory;
    private readonly IGameAudio audio;

    public CompoundTileHandler(ICollection<IBlock> blocks, ICollection<IEnemy> enemies,
        SpriteFactory spriteFactory, IEntitySpawner spawner, ItemFactory itemFactory, IGameAudio audio)
    {
        this.blocks = blocks;
        this.enemies = enemies;
        this.spriteFactory = spriteFactory;
        this.spawner = spawner;
        this.itemFactory = itemFactory;
        this.audio = audio;
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
                    new Block(spriteFactory, spawner, itemFactory, audio,
                        BlockType.UsedQuestion, 
                        position));
                return true;
            case "Bbl":
                enemies.Add(
                    new BillLauncher(
                        null,
                        position, spawner));
                blocks.Add(
                    new Block(spriteFactory, spawner, itemFactory, audio, BlockType.LauncherHead, position));
                return true;
            default:
                return false;
        }
    }
}
