using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;

namespace Mario.Game.Physics;

public static class CollisionManager
{
    public record struct CollisionResult(Vector2 Normal, float Penetration);

    // Resolve one pair and notify both colliders after correction.
    // Missing previous bounds use the collider's current bounds.
    public static void ResolveCollision(ICollider a, ICollider b,
        Box? prevA = null, Box? prevB = null)
    {
        if (ReferenceEquals(a, b) || !CanCollide(a) || !CanCollide(b))
            return;

        var result = CheckCollision(a.Bounds, b.Bounds, prevA, prevB);
        if (!result.HasValue)
            return;

        Vector2 relativeVelocity = GetRelativeVelocity(a, b);
        Vector2 normal = result.Value.Normal;
        // Ignore pass-through contacts before notifying either collider.
        if (!AcceptsContact(b, a, normal, relativeVelocity, prevB, prevA) ||
            !AcceptsContact(a, b, -normal, -relativeVelocity, prevA, prevB))
            return;

        // Current solids are static terrain; dynamic solid response is unsupported.
        if (a is IPhysicsBody bodyA && a.BlockingMode == ICollider.BlockingModes.None &&
            b.BlockingMode != ICollider.BlockingModes.None && b.BlockingMode != ICollider.BlockingModes.ThroughWalls)
            ResolveBody(bodyA, normal, result.Value.Penetration);
        else if (b is IPhysicsBody bodyB && b.BlockingMode == ICollider.BlockingModes.None &&
                 a.BlockingMode != ICollider.BlockingModes.None && a.BlockingMode != ICollider.BlockingModes.ThroughWalls)
            ResolveBody(bodyB, -normal, result.Value.Penetration);

        a.OnCollision(new CollisionEvent(b, normal, relativeVelocity));
        b.OnCollision(new CollisionEvent(a, -normal, -relativeVelocity));
    }

    // Normal and velocity are expressed from the other collider's perspective.
    private static bool AcceptsContact(ICollider surface, ICollider other,
        Vector2 normal, Vector2 relativeVelocity, Box? previousSurface, Box? previousOther)
    {
        if (surface.BlockingMode != ICollider.BlockingModes.TopOnly)
            return true;

        Box oldSurface = previousSurface ?? surface.Bounds;
        Box oldOther = previousOther ?? other.Bounds;
        return normal.Y < 0f && relativeVelocity.Y >= 0f &&
               oldOther.Bottom <= oldSurface.Top + 0.1;
    }

    private static bool CanCollide(ICollider collider)
    {
        return collider.CollisionEnabled && (collider is not IGameObject obj || obj.Active);
    }

    private static Vector2 GetRelativeVelocity(ICollider a, ICollider b)
    {
        Vector2 velocityA = a is IPhysicsBody bodyA ? bodyA.Velocity : Vector2.Zero;
        Vector2 velocityB = b is IPhysicsBody bodyB ? bodyB.Velocity : Vector2.Zero;
        return velocityA - velocityB;
    }

    private static void ResolveBody(IPhysicsBody body, Vector2 normal, float penetration)
    {
        body.Position += normal * penetration;
        float normalSpeed = Vector2.Dot(body.Velocity, normal);
        if (normalSpeed < 0f)
            body.Velocity -= normal * normalSpeed;
        if (normal.Y < 0f && normalSpeed <= 0f)
            body.IsOnGround = true;
    }

    // Normal points from B toward A;  return null is no collision
    // Edge contact counts; corner-only contact does not.
    //
    // This is not a full swept AABB test: side crossings require overlap on
    // the other axis at the final position. A diagonal pass-through can be
    // missed when neither axis overlaps at the end of the step, even if the
    // rectangles intersected during movement.
    public static CollisionResult? CheckCollision(Box a, Box b, Box? prevA, Box? prevB)
    {
        if (a.Width <= 0 || a.Height <= 0 || b.Width <= 0 || b.Height <= 0)
            return null;

        var oldA = prevA ?? a;
        var oldB = prevB ?? b;

        bool horizontalOverlap = a.Left < b.Right && a.Right > b.Left;
        bool verticalOverlap = a.Top < b.Bottom && a.Bottom > b.Top;
        // A landing or head bump only counts if A was already above or below B last step.
        // Without this, pushing a few pixels into a wall while falling past a seam
        // between stacked blocks looks like landing on the lower block.
        bool wasHorizontallyOverlapping = oldA.Left < oldB.Right && oldA.Right > oldB.Left;

        // Prefer the side crossed since the previous step.
        if (horizontalOverlap && wasHorizontallyOverlapping)
        {
            if (oldA.Bottom <= oldB.Top && a.Bottom >= b.Top)
                return new CollisionResult(-Vector2.UnitY, a.Bottom - b.Top);
            if (oldA.Top >= oldB.Bottom && a.Top <= b.Bottom)
                return new CollisionResult(Vector2.UnitY, b.Bottom - a.Top);
        }

        if (verticalOverlap)
        {
            if (oldA.Right <= oldB.Left && a.Right >= b.Left)
                return new CollisionResult(-Vector2.UnitX, a.Right - b.Left);
            if (oldA.Left >= oldB.Right && a.Left <= b.Right)
                return new CollisionResult(Vector2.UnitX, b.Right - a.Left);
        }

        if (!(horizontalOverlap && verticalOverlap))
            return null;

        return FallbackCollision(a, b);
    }

    // If the entry side is unknown, use the shortest distance to separate overlapping bounds.
    private static CollisionResult? FallbackCollision(Box a, Box b)
    {
        CollisionResult? result = null;
        Consider(-Vector2.UnitY, a.Bottom - b.Top, ref result);
        Consider(Vector2.UnitY, b.Bottom - a.Top, ref result);
        Consider(-Vector2.UnitX, a.Right - b.Left, ref result);
        Consider(Vector2.UnitX, b.Right - a.Left, ref result);
        return result;
    }

    private static void Consider(Vector2 normal, float penetration, ref CollisionResult? result)
    {
        if (!result.HasValue || penetration < result.Value.Penetration)
            result = new CollisionResult(normal, penetration);
    }
}