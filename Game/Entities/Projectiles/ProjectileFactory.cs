using System;
using Mario.Game.Entities.Enemies;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;

namespace Mario.Game.Entities.Projectiles;

public sealed class ProjectileFactory
{
    private const float FireballLifetime = 2f;
    private const float StandardProjectileLifetime = 3f;

    private readonly ISpriteFactory sprites;
    private readonly IEntitySpawner spawner;

    public ProjectileFactory(ISpriteFactory sprites, IEntitySpawner spawner)
    {
        this.sprites = sprites;
        this.spawner = spawner;
    }

    // Each request gets independent animation state and type-specific defaults.
    public IGameObject Create(ProjectileType type, Vector2 position, Vector2 velocity) => type switch
    {
        ProjectileType.Fireball => new Fireball(
            sprites.Create("projectiles", "fireball"), position, velocity,
            FireballLifetime, GameConstants.Projectiles.DefaultBounceSpeed),
        ProjectileType.Hammer => new Hammer(
            sprites.Create("projectiles", "hammer"), position, velocity,
            StandardProjectileLifetime),
        ProjectileType.Fire => new Fire(
            sprites.Create("projectiles", "fire"), position, velocity,
            StandardProjectileLifetime),
        ProjectileType.Egg => new Egg(
            sprites.Create("projectiles", "egg"), position, velocity,
            StandardProjectileLifetime, spawner, sprites),
        ProjectileType.BulletBill => new BulletBill(
            sprites.Create("enemies", "bulletBill"), position, velocity),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown projectile type"),
    };
}
