using Mario.Game.Physics;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Projectiles;

public class Hammer : IProjectile
{
    private ISprite sprite;
    private float lifeTime;
    private float elapsedTime = 0f;
    private float deathY;

    public Vector2 PendingAcceleration { get; set; }
    public bool IsOnGround { get; set; }
    public bool CollisionEnabled => false;
    public Vector2 Position { get; set; }
    public ICollider.BlockingModes BlockingMode => ICollider.BlockingModes.None;
    public Box Bounds => new Box(Position.X, Position.Y, 32, 32);
    private bool active;
    public bool Active => active && Position.Y < deathY;
    public Vector2 Velocity { get; set; }

    public Hammer(ISprite sprite, Vector2 position, Vector2 velocity, float lifeTime, float deathY)
    {
        this.sprite = sprite;
        Position = position;
        Velocity = velocity;
        this.lifeTime = lifeTime;
        this.deathY = deathY;

        active = true;
    }

    public void Update(GameTime gameTime)
    {
        if (!Active) return;
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        elapsedTime += deltaTime;
        if (elapsedTime > lifeTime)
        {
            active = false;
            return;
        }

        if (Position.Y >= deathY)
        {
            active = false;
        }

        sprite.Play("Idle");
        sprite.UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, Position);
    }
}
