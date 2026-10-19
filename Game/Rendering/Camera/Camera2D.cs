using Microsoft.Xna.Framework;
using System;

namespace Mario.Game.Rendering.Camera;

public class Camera2D
{
    private readonly float zoom;

    public Camera2D(float zoom = 1.6f)
    {
        this.zoom = zoom;
    }

    public Vector2 Position { get; private set; }

    public Matrix Transform =>
        Matrix.CreateTranslation(
            -Position.X,
            -Position.Y,
            0f) * Matrix.CreateScale(zoom);

    public void Follow(Vector2 playerPosition, int screenWidth, int levelWidth)
    {
        float visibleWidth = screenWidth / zoom;
        float targetX = playerPosition.X - visibleWidth / 2f;
        float maxX = Math.Max(0, levelWidth - visibleWidth);

        Position = new Vector2(
            MathHelper.Clamp(targetX, 0, maxX),
            0);
    }
}