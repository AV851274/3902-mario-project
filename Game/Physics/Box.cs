namespace Mario.Game.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Game.Physics;
using System;
using Mario.Game.Interfaces;
using Mario.Game.Entities.Items;
using Mario.Game.Interfaces.Player;

using Mario.Game.Rendering.Sprites;

// Floating-point axis-aligned bounds for collision detection.
public readonly record struct Box
{
    public Box(float X, float Y, float Width, float Height)
    {
        this.X = X;
        this.Y = Y;
        this.Width = Width;
        this.Height = Height;
    }

    public float Left => X;
    public float Right => X + Width;
    public float Top => Y;
    public float Bottom => Y + Height;
    public float X { get; init; }
    public float Y { get; init; }
    public float Width { get; init; }
    public float Height { get; init; }

    // Edge-only contact does not count as overlap.
    public bool Intersects(Box other)
    {
        return Left < other.Right && Right > other.Left &&
               Top < other.Bottom && Bottom > other.Top;
    }
}