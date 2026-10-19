using System;
using System.Collections.Generic;
using System.Linq;
using Mario.Game.Interfaces;
using Mario.Game.State;
using Microsoft.Xna.Framework;

namespace Mario.Game.Physics;

public sealed class PhysicsEngine
{
    public float Gravity { get; }

    public PhysicsEngine(float gravity)
    {
        ValidateNonNegative(gravity, nameof(gravity));
        Gravity = gravity;
    }

    // Entity Update prepares actions/acceleration; Step owns integration and collisions.
    public void Step(GameEntities entities, GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        ValidateNonNegative(deltaTime, nameof(gameTime));
        if (deltaTime <= float.Epsilon)
            return;

        var previousBounds = UpdateBodies(entities, deltaTime);
        ResolveTerrainCollisions(entities, previousBounds);
        ResolveProjectileInteractions(entities, previousBounds);
        ResolvePlayerInteractions(entities, previousBounds);
    }

    private Dictionary<ICollider, Box?> UpdateBodies(GameEntities entities, float deltaTime)
    {
        var bodies = entities.GetAllGameObjects()
            .Where(gameObj => gameObj.Active)
            .OfType<IPhysicsBody>()
            .Distinct();

        var previousBounds = new Dictionary<ICollider, Box?>();
        foreach (IPhysicsBody body in bodies)
        {
            previousBounds.Add(body, body.Bounds);

            // apply acceleration
            Vector2 velocity = body.Velocity + body.PendingAcceleration * deltaTime;

            // apply gravity
            velocity.Y += Gravity * body.GravityScale * deltaTime;
            if (body.MaxFallSpeed > 0f && velocity.Y > body.MaxFallSpeed)
                velocity.Y = body.MaxFallSpeed;

            // update body
            body.Velocity = velocity;
            body.Position += velocity * deltaTime;
            body.IsOnGround = false;
            body.PendingAcceleration = Vector2.Zero;
        }

        return previousBounds;
    }

    private static void ResolveTerrainCollisions(GameEntities entities,
        Dictionary<ICollider, Box?> previousBounds)
    {
        var solids = entities.GetAllGameObjects()
            .Where(obj => obj.Active)
            .OfType<ICollider>()
            .Where(collider => collider.IsSolid && collider is not IPhysicsBody)
            .Distinct()
            .ToArray();

        foreach (var body in previousBounds.Keys)
        {
            // Land before checking walls, so floor seams are not treated as walls.
            for (int axis = 0; axis < 2; axis++)
            {
                foreach (var solid in solids)
                {
                    var collision = CollisionManager.CheckCollision(body.Bounds, solid.Bounds,
                        previousBounds[body], null);
                    if (collision.HasValue && (collision.Value.Normal.Y != 0f) == (axis == 0))
                        ResolveCollision(body, solid, previousBounds);
                }
            }
        }
    }

    private static void ResolveProjectileInteractions(GameEntities entities,
        Dictionary<ICollider, Box?> previousBounds)
    {
        foreach (var projectile in entities.Projectiles.Distinct())
            foreach (var enemy in entities.Enemies.Distinct())
                ResolveCollision(projectile, enemy, previousBounds);
    }

    private static void ResolvePlayerInteractions(GameEntities entities,
        Dictionary<ICollider, Box?> previousBounds)
    {
        foreach (var player in entities.Players.Distinct())
        {
            foreach (var item in entities.Items.Distinct())
                ResolveCollision(player, item, previousBounds);
            foreach (var enemy in entities.Enemies.Distinct())
                ResolveCollision(player, enemy, previousBounds);
        }
    }

    private static void ResolveCollision(ICollider a, ICollider b,
        Dictionary<ICollider, Box?> previousBounds)
    {
        CollisionManager.ResolveCollision(a, b,
            previousBounds.GetValueOrDefault(a), previousBounds.GetValueOrDefault(b));
    }

    private static void ValidateNonNegative(float value, string name)
    {
        if (!float.IsFinite(value) || value < 0f)
            throw new ArgumentOutOfRangeException(name);
    }
}
