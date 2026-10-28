using Mario.Game.Physics;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Game.Interfaces.Player;
using System;

namespace Mario.Game.Entities.Projectiles;

public class Egg : IProjectile
{
    private ISprite sprite;
    private float lifeTime;
    private float elapsedTime = 0f;
    private float deathY = 1000f;

    public Vector2 PendingAcceleration { get; set; }
    public bool IsOnGround { get; set; }
    public bool CollisionEnabled => true;
    public Vector2 Position { get; set; }
    public ICollider.BlockingModes BlockingMode => ICollider.BlockingModes.None;
    public Box Bounds => new Box(Position.X, Position.Y, 40, 40);
    public bool Active { get; private set; }
    public Vector2 Velocity { get; set; }

    public IGameObject.DrawLayer layer => IGameObject.DrawLayer.Effects;

    public event Action<Egg> SummonSpiny;

    public Egg(ISprite sprite, Vector2 position, Vector2 velocity, float lifeTime)
    {
        this.sprite = sprite;
        Position = position;
        Velocity = velocity;
        this.lifeTime = lifeTime;

        Active = true;
    }

    public void Update(GameTime gameTime)
    {
        if (!Active) return;
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        elapsedTime += deltaTime;
        if (elapsedTime > lifeTime)
        {
            Active = false;
            return;
        }

        if (Position.Y >= deathY)
        {
            Active = false;
        }

        if (IsOnGround)
        {
            SummonSpiny?.Invoke(this);
            Active = false;
        }

        sprite.Play("Idle");
        sprite.UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (Active)
            sprite.Draw(spriteBatch, Position);
    }

        public void OnCollision(CollisionEvent collision)
    {
        if (!Active)
            return;

        if (collision.Other is IPlayer player)
        {
            player.TakeDamage();
        }
    }
}
