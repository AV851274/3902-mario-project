using Mario.Game.Interfaces;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Mario.Game.Physics;

namespace Mario.Game.Entities.Enemies;


    
public class FireBar : StaticEnemyBase
{
    private float angle; //angle in radians
    private const int Width = 24;
    private const int Height = 24;

    private const float RotationSpeed = 2f;
    private Vector2[] fireballPositions;
    private Vector2[] previousFireballPositions;

    public override bool canBeStomped => false;

    public FireBar(ISprite sprite, Vector2 initialPosition, int length)
        : base(sprite, initialPosition, Width, Height)
    {
        if (length <= 1)
            throw new ArgumentOutOfRangeException(nameof(length), "FireBar length must be at least 2.");

        fireballPositions = new Vector2[(length - 1)];
        previousFireballPositions = new Vector2[fireballPositions.Length];
        UpdateFireballPositions();
        Array.Copy(fireballPositions, previousFireballPositions, fireballPositions.Length);
    }

    public IReadOnlyList<Box> GetFireballBounds()
    {
        Box[] bounds = new Box[fireballPositions.Length];
        for (int i = 0; i < fireballPositions.Length; i++)
            bounds[i] = new Box(fireballPositions[i].X, fireballPositions[i].Y, Width, Height);

        return bounds;
    }

    public IReadOnlyList<Box> GetPreviousFireballBounds()
    {
        Box[] bounds = new Box[previousFireballPositions.Length];
        for (int i = 0; i < previousFireballPositions.Length; i++)
            bounds[i] = new Box(previousFireballPositions[i].X, previousFireballPositions[i].Y, Width, Height);

        return bounds;
    }

    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        angle += RotationSpeed * deltaTime;

        Array.Copy(fireballPositions, previousFireballPositions, fireballPositions.Length);
        UpdateFireballPositions();

        sprite.UpdateAnimation(gameTime);
    }

    private void UpdateFireballPositions()
    {
        Vector2 direction = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        for (int i = 0; i < fireballPositions.Length; i++)
            fireballPositions[i] = Position + direction * Width * (i + 1);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, Position);
        foreach (Vector2 fireballPosition in fireballPositions)
            sprite.Draw(spriteBatch, fireballPosition);
    }

    public override void Stomp()
    {
    }

}