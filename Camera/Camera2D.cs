using Microsoft.Xna.Framework;
using System;

namespace Mario.Camera;

public class Camera2D
{
    public Vector2 Position { get; private set; }

    public Matrix Transform =>
        Matrix.CreateTranslation(
            -Position.X,
            -Position.Y,
            0f);

    public void Follow(Vector2 playerPosition, int screenWidth, int levelWidth)
    {
        float targetX = playerPosition.X - screenWidth / 2f;
        float maxX = Math.Max(0, levelWidth - screenWidth);

        Position = new Vector2(
            MathHelper.Clamp(targetX, 0, maxX),
            0);
    }
}