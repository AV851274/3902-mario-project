using System.Collections.Generic;
using Mario.Game.Entities.Enemies;
using Mario.Game.Interfaces;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;
using static Mario.Game.Entities.Enemies.EnemyBase;

namespace Mario.Game.Levels;

internal sealed class EnemyTileHandler : ILevelTileHandler
{
    private readonly ICollection<IEnemy> enemies;
    private readonly SpriteFactory spriteFactory;
    private readonly IEntitySpawner spawner;

    public EnemyTileHandler(ICollection<IEnemy> enemies, SpriteFactory spriteFactory, IEntitySpawner spawner)
    {
        this.enemies = enemies;
        this.spriteFactory = spriteFactory;
        this.spawner = spawner;
    }

    public bool Place(string tile, Vector2 position)
    {
        switch (tile)
        {
            case "E":
                enemies.Add(
                    new Goomba(
                        spriteFactory.Create("enemies", "goomba"),
                        position));
                return true;
            case "K":
                enemies.Add(
                    new Turtle(
                        spriteFactory.Create("enemies", "turtle"),
                        position));
                return true;
            case "H":
                enemies.Add(
                    new HammerBro(
                        spriteFactory.Create("enemies", "hammerBro"),
                        position, spawner));
                return true;
            case "Bw":
                enemies.Add(
                    new Bowser(
                        spriteFactory.Create("enemies", "bowser"),
                        position, spawner));
                return true;
            case "Lk":
                enemies.Add(
                    new Lakitu(
                        spriteFactory.Create("enemies", "lakitu"),
                        position, spawner));
                return true;
            case "Bz":
                enemies.Add(
                    new BuzzyBeetle(
                        spriteFactory.Create("enemies", "buzzyBeetle"),
                        position));
                return true;
            case "Sp":
                enemies.Add(
                    new Spiny(
                        spriteFactory.Create("enemies", "spiny"),
                        position));
                return true;
            case "BPb":
                enemies.Add(
                    new Podoboo(
                        spriteFactory.Create("enemies", "podoboo"),
                        position, -1000f));
                return true;
            case "Pb":
                enemies.Add(
                    new Podoboo(
                        spriteFactory.Create("enemies", "podoboo"),
                        position, -800f));
                return true;
            case "Pk":
                enemies.Add(
                    new ParaKoopa(
                        spriteFactory.Create("enemies", "paraKoopa"),
                        position, spawner, spriteFactory));
                return true;
            case "GrayCheep":
                enemies.Add(
                    new CheepCheep(
                        spriteFactory.Create("enemies", "grayCheep"),
                        position,
                        FishColor.Gray));
                return true;
            case "GreenCheep":
                enemies.Add(
                    new CheepCheep(
                        spriteFactory.Create("enemies", "greenCheep"),
                        position,
                        FishColor.Green));
                return true;
            default:
                return false;
        }
    }
}
