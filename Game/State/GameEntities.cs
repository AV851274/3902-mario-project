using System;
using Mario.Game.Entities.Projectiles;
using Mario.Game.Interfaces.Player;
using Mario.Game.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Mario.Game.State;

public class GameEntities
{
    private const int MaxFireballs = 4;
    private readonly List<IGameObject> pendingSpawns = new();

    public List<IPlayer> Players { get; } = new();
    public List<IBlock> Blocks { get; } = new();
    public List<IItem> Items { get; } = new();
    public List<IEnemy> Enemies { get; } = new();
    public List<IProjectile> Projectiles { get; } = new();
    public List<IGameObject> Background { get; } = new();

    public void QueueSpawn(IGameObject entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        pendingSpawns.Add(entity);
    }

    public bool TrySpawnProjectile(
        ProjectileType type, Vector2 position, Vector2 velocity, ProjectileFactory factory)
    {
        // Reject before constructing the projectile or allocating its sprite.
        if (type == ProjectileType.Fireball && CountActive<Fireball>() >= MaxFireballs)
            return false;

        QueueSpawn(factory.Create(type, position, velocity));
        return true;
    }

    // Include queued entities so limits still apply before the end-of-frame commit.
    public int CountActive<T>() where T : IGameObject =>
        GetAllGameObjects().Concat(pendingSpawns).OfType<T>().Count(entity => entity.Active);

    public void CommitSpawns()
    {
        foreach (IGameObject entity in pendingSpawns)
        {
            if (!entity.Active)
                continue;

            switch (entity)
            {
                case IPlayer player: Players.Add(player); break;
                case IBlock block: Blocks.Add(block); break;
                case IItem item: Items.Add(item); break;
                case IEnemy enemy: Enemies.Add(enemy); break;
                case IProjectile projectile: Projectiles.Add(projectile); break;
                default: Background.Add(entity); break;
            }
        }
        pendingSpawns.Clear();
    }

    public IEnumerable<IGameObject> GetAllGameObjects()
    {
        return Background
            .Concat(Items)
            .Concat(Blocks)
            .Concat(Enemies)
            .Concat(Projectiles)
            .Concat(Players);
    }

    public IEnumerable<ICollider> GetAllColliders()
    {
        return GetAllGameObjects().OfType<ICollider>();
    }

    public void Update(GameTime gameTime)
    {
        Players.RemoveAll(p => !p.Active);
        Blocks.RemoveAll(b => !b.Active);
        Items.RemoveAll(i => !i.Active);
        Enemies.RemoveAll(e => !e.Active);
        Projectiles.RemoveAll(p => !p.Active);
        Background.RemoveAll(b => !b.Active);

        foreach (var obj in GetAllGameObjects())
        {
            obj.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Keep drawing order separate from the simulation order above.
        foreach (IGameObject obj in Background) obj.Draw(spriteBatch);
        foreach (IItem item in Items) item.Draw(spriteBatch);
        foreach (IBlock block in Blocks) block.Draw(spriteBatch);
        foreach (IEnemy enemy in Enemies) enemy.Draw(spriteBatch);
        foreach (IPlayer player in Players) player.Draw(spriteBatch);
        foreach (IProjectile projectile in Projectiles) projectile.Draw(spriteBatch);
    }

    public void Reset()
    {
        pendingSpawns.Clear();
        Players.Clear();
        Blocks.Clear();
        Items.Clear();
        Enemies.Clear();
        Projectiles.Clear();
        Background.Clear();
    }
}
