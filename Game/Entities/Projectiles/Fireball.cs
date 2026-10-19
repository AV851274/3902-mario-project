using Mario.Game.Physics;
using System;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Projectiles;

public class Fireball : IProjectile
{
    private ISprite sprite;
    private float lifeTime;
    private float elapsedTime = 0f;

    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 PendingAcceleration { get; set; }
    public bool IsOnGround { get; set; }
    public float BounceSpeed { get; }
    public bool CollisionEnabled => Active;
    public ICollider.BlockingModes BlockingMode => ICollider.BlockingModes.None;
    public Box Bounds => new Box(Position.X, Position.Y, 64, 64);
    public bool Active { get; private set; }

    public Fireball(
        ISprite sprite,
        Vector2 position,
        Vector2 velocity,
        float lifeTime,
        float bounceSpeed = 400f)
    {
        this.sprite = sprite;
        Position = position;
        Velocity = velocity;
        this.lifeTime = lifeTime;
        if (!float.IsFinite(bounceSpeed) || bounceSpeed < 0f)
            throw new ArgumentOutOfRangeException(nameof(bounceSpeed));
        BounceSpeed = bounceSpeed;
        Active = true;
    }

    public void Update(GameTime gameTime)
    {
        if (!Active)
            return;

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        elapsedTime += deltaTime;
        if (elapsedTime > lifeTime)
        {
            Active = false;
            return;
        }

        sprite.Play("Idle");
        sprite.UpdateAnimation(gameTime);
    }

    public void OnCollision(CollisionEvent collision)
    {
        if (!Active)
            return;

        if (collision.Other.BlockingMode != ICollider.BlockingModes.None)
        {
            if (collision.Normal.Y < 0f)
            {
                if (IsOnGround && BounceSpeed > 0f)
                {
                    Velocity = new Vector2(Velocity.X, -BounceSpeed);
                    IsOnGround = false;
                }
            }
            else
                Active = false;
        }
        else if (collision.Other is IEnemy enemy)
        {
            enemy.HitByFireball();
            Active = false;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (Active)
            sprite.Draw(spriteBatch, Position);
    }
}