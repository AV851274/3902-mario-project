using Mario.Game.Physics;
using System;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Metadata;

namespace Mario.Game.Entities.Projectiles;

public class Fireball : IProjectile
{
    private ISprite sprite;
    private float lifeTime;
    private float timeOfDeath;
    private bool dead = false;
    private float elapsedTime = 0f;

    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 PendingAcceleration { get; set; }
    public bool IsOnGround { get; set; }
    public float BounceSpeed { get; }
    public bool CollisionEnabled => Active && !dead;
    public ICollider.BlockingModes BlockingMode => ICollider.BlockingModes.None;
    public Box Bounds => new Box(Position.X, Position.Y, 24, 24);
    public bool Active { get; private set; }

    public float GravityScale => dead ? 0f : 1f;

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
        if(!Active)
            return;

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += deltaTime;
        if (dead) {
            if (elapsedTime - timeOfDeath < 0.15f)
            {
                sprite.Play("Explode");
                sprite.UpdateAnimation(gameTime);
            } else {
                Active = false;
            }

        } else {

            if (elapsedTime > lifeTime)
            {
                Explode();
                return;
            }

            sprite.Play("Idle");
            sprite.UpdateAnimation(gameTime);
        } 
    }

    private void Explode()
    {
        if (!dead) {
            Velocity = Vector2.Zero;
            dead = true;
            timeOfDeath = elapsedTime; 
        }
    }

    public void OnCollision(CollisionEvent collision)
    {
        if (!Active)
            return;

        if (collision.Other.BlockingMode != ICollider.BlockingModes.None
        && collision.Other.BlockingMode != ICollider.BlockingModes.ThroughWalls)
        {
            if (collision.Normal.Y < 0f)
            {
                if (IsOnGround && BounceSpeed > 0f)
                {
                    Velocity = new Vector2(Velocity.X, -300f);
                    IsOnGround = false;
                }
            }
            else {
                Explode();
            }
        }
        else if (collision.Other is IEnemy enemy)
        {
            enemy.HitByFireball();
            Explode();
        }

    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (Active) {
            if(dead)
            {
                sprite.Draw(spriteBatch, new Vector2(Position.X - 8, Position.Y - 8));
            } else {
                SpriteEffects flip = Velocity.X < 0f ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                sprite.Draw(spriteBatch, Position, (float)flip);
            }
        }
    }
}