using Microsoft.Xna.Framework;
using System;

namespace Mario.Camera;

public class Camera2D
{
    private const float Zoom = 1.6f;

    public Vector2 Position { get; private set; }

    public Matrix Transform =>
        Matrix.CreateTranslation(
            -Position.X,
            -Position.Y,
            0f) * Matrix.CreateScale(Zoom);

    public void Follow(Vector2 playerPosition, int screenWidth, int levelWidth)
    {
        float visibleWidth = screenWidth / Zoom;
        float targetX = playerPosition.X - visibleWidth / 2f;
        float maxX = Math.Max(0, levelWidth - visibleWidth);

        Position = new Vector2(
            MathHelper.Clamp(targetX, 0, maxX),
            0);
    }
}