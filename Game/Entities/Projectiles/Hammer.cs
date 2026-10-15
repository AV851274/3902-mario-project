using Mario.Interfaces;
using Mario.Physics;
using Mario.Physics.Aspects;
using Mario.Physics.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Projectiles;

public class Hammer : IProjectile
{
    private ISprite sprite;
    private float lifeTime;
    private float elapsedTime = 0f;
    private float deathY;
    private readonly PhysicsController physicsController;
    private PhysicsState physicsState;

    public Vector2 Position => physicsState.Position;
    public bool Active { get; private set; }
    public Vector2 Velocity => physicsState.Velocity;


    public Hammer(ISprite sprite, Vector2 position, Vector2 velocity, float lifeTime, float gravity, float deathY)
    {
        this.sprite = sprite;
        physicsState = new PhysicsState(position, velocity);
        this.lifeTime = lifeTime;
        this.deathY = deathY;
        physicsController = new PhysicsController
        {
            GravityPhysics = new GravityPhysics(gravity),
            MovementPhysics = new MovementPhysics()
        };
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

        physicsController.Update(ref physicsState, deltaTime);
        if (Position.Y >= deathY)
        {
            Active = false;
        }

        sprite.Play("Idle");
        sprite.UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, Position);
    }
}