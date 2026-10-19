using Microsoft.Xna.Framework;

namespace Mario.Game.Interfaces;

// Normal points from Other toward this receiver. The opposite receiver gets -Normal.
// Relative velocity is captured before collision resolution changes either velocity.
public readonly record struct CollisionEvent(
    ICollider Other,
    Vector2 Normal,
    Vector2 RelativeVelocityBefore);
