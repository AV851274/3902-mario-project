using System;
using Mario.Game.Rendering.Camera;
using Mario.Game.Rendering.Sprites;
using Mario.Game.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Rendering;

public sealed class WorldRenderer : IDisposable
{
    private const int WaterTopY = 48;
    private static readonly Color WaterColor = new(0x42, 0x42, 0xFF);

    private readonly GraphicsDevice graphicsDevice;
    private readonly SpriteBatch spriteBatch;
    private readonly HitboxRenderer hitboxes;
    private readonly Hud hud;
    private readonly Texture2D pixel;
    private Camera2D camera = new();
    private int levelWidth;
    private int levelHeight;
    private bool underwater;
    private bool showHitboxes;

    public WorldRenderer(
        GraphicsDevice graphicsDevice,
        SpriteBatch spriteBatch,
        HitboxRenderer hitboxes,
        Hud hud)
    {
        this.graphicsDevice = graphicsDevice;
        this.spriteBatch = spriteBatch;
        this.hitboxes = hitboxes;
        this.hud = hud;
        pixel = new Texture2D(graphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });
    }

    public void ConfigureRoom(int width, int height, bool underwater)
    {
        levelWidth = width;
        levelHeight = height;
        this.underwater = underwater;
        // Recreate the camera to reset its position and fit the room height.
        camera = new Camera2D(graphicsDevice.Viewport.Height / (float)height);
    }

    public void Follow(Vector2 playerPosition) =>
        camera.Follow(playerPosition, graphicsDevice.Viewport.Width, levelWidth);

    public void ToggleHitboxes() => showHitboxes = !showHitboxes;

    public void Draw(GameEntities entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        graphicsDevice.Clear(Color.CornflowerBlue);

        spriteBatch.Begin(
            SpriteSortMode.Deferred,
            null,
            SamplerState.PointClamp,
            null,
            null,
            null,
            camera.Transform);

        // Water is behind entities and uses the world camera transform.
        if (underwater)
            spriteBatch.Draw(pixel, new Rectangle(0, WaterTopY, levelWidth, levelHeight - WaterTopY), WaterColor);

        entities.Draw(spriteBatch);

        if (showHitboxes)
        {
            foreach (var collider in entities.GetAllColliders())
            {
                hitboxes.DrawHitbox(spriteBatch, collider.Bounds.Left, collider.Bounds.Right,
                    collider.Bounds.Top, collider.Bounds.Bottom);
            }
        }

        spriteBatch.End();

        // HUD stays fixed on screen, outside the world camera transform.
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        hud.Draw(spriteBatch, graphicsDevice.Viewport.Width);
        spriteBatch.End();
    }

    public void Dispose() => pixel.Dispose();
}
