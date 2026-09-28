using Game2D.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Fireball : IProjectile
{
    private ISprite sprite;
    private float lifeTime;
    private float elapsedTime = 0f;
    private float gravity;
    private float groundY;

    public Vector2 Position { get; private set; }
    public bool Active { get; private set; }
    public Vector2 Velocity { get; private set; }


    public Fireball(ISprite sprite, Vector2 position, Vector2 velocity, float lifeTime, float gravity, float groundY)
    {
        this.sprite = sprite;
        this.Position = position;
        this.lifeTime = lifeTime;
        this.Velocity = velocity;
        this.gravity = gravity;
        this.groundY = groundY;
        Active = true;
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        elapsedTime += deltaTime;
        if (elapsedTime > lifeTime)
        {
            Active = false;
            return;
        }

        Velocity += new Vector2(0, gravity * deltaTime);
        Position += Velocity * deltaTime;
        if (Position.Y >= groundY)
        {
            Velocity *= new Vector2(1, -1);
            Position = new Vector2(Position.X, groundY - 0.1f);
        }

        sprite.Play("Idle");
        sprite.UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, Position);
    }
}