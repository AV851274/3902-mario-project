using System;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public class Podoboo : EnemyBase
{
    private const int Width = 40;
    private const int Height = 40;
    private const float waitingY = 720f;
    private float jumpSpeed;
    private bool waiting = false;
    public override float GravityScale => waiting? 0f : 1f;

    public override bool canBeStomped => false;
    public override ICollider.BlockingModes BlockingMode => ICollider.BlockingModes.ThroughWalls;

    public Podoboo(ISprite sprite, Vector2 initialPosition, float jumpSpeed)
        : base(sprite, initialPosition, Width, Height)
    {
        this.jumpSpeed = jumpSpeed;
        Position = initialPosition;
        Velocity = Vector2.Zero;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = Velocity.Y > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None;
        sprite.Draw(spriteBatch, Position, effects: effects);
    }

    public override void Update(GameTime gameTime)
    {

        if(Position.Y >= waitingY)
        {
            Velocity = Vector2.Zero;
            waiting = true;
        }

        if (waiting && Random.Shared.NextDouble() < 0.005) // 2% chance to jump each frame
            {
                Velocity = new Vector2(0f, jumpSpeed);
                waiting = false;
            }

        sprite.UpdateAnimation(gameTime);
    }

    public override void HitByFireball()
    {
        return;
    }

    public override void Stomp()
    {
        if (DefeatedByFireball)
            return;
    }

    protected override void OnWallBounce(float RelativeVelocityBeforeX)
    {
        Velocity = new Vector2(-RelativeVelocityBeforeX, Velocity.Y);
    }
}