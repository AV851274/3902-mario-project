using Mario.Game.Entities.Projectiles;
using Microsoft.Xna.Framework;

namespace Mario.Game.Interfaces;

/// <summary>
/// Registers entities and handles projectile creation requests.
/// </summary>
/// <remarks>
/// Entities request projectiles by type, position, and velocity. GameEntities checks
/// active and queued counts before ProjectileFactory creates the sprite and object
/// with type-specific defaults. Non-projectile objects use Spawn; their sprite
/// creation is a separate dependency.
/// </remarks>
public interface IEntitySpawner
{
    void Spawn(IGameObject entity);
    bool TrySpawnProjectile(ProjectileType type, Vector2 position, Vector2 velocity);
}