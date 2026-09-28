using Game2D.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Fireball : IProjectile
{
    private ISprite sprite;
    private float lifeTime;
    private float elapsedTime = 0f;

    public Vector2 Position { get; private set; }
    public bool Active { get; private set; }
    public Vector2 Velocity { get; private set; }


    public Fireball(ISprite sprite, Vector2 position, Vector2 velocity, float lifeTime)
    {
        this.sprite = sprite;
        this.Position = position;
        this.lifeTime = lifeTime;
        this.Velocity = velocity;
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

        Position += Velocity * deltaTime;
        sprite.Play("Idle");
        sprite.UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, Position );
    }
}