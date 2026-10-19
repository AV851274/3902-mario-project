using Mario.Game.Physics;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public class Goomba : IEnemy, IPhysicsBody
{
    private const float WalkSpeed = 60f;
    private const int Size = 32;

    private ISprite sprite;

    private float leftBound;
    private float rightBound;
    private bool isStomped;

    public Vector2 PendingAcceleration { get; set; }
    public bool IsOnGround { get; set; }
    public float GravityScale => 1f;
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }

    public Box Bounds => new Box(Position.X, Position.Y, Size, Size);
    public bool Active => Position.Y <= 1000f;
    public bool CollisionEnabled => Active && !isStomped;
    public bool IsDefeated => isStomped;
    public bool CanHurtPlayer => !isStomped;

    public Goomba(ISprite sprite, Vector2 initialPosition, int patrolDistance = 150)
    {
        this.sprite = sprite;
        Position = initialPosition;
        Velocity = new Vector2(-WalkSpeed, 0f);
        leftBound = initialPosition.X - patrolDistance;
        rightBound = initialPosition.X + patrolDistance;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = isStomped ? SpriteEffects.FlipVertically : SpriteEffects.None;
        sprite.Draw(spriteBatch, Position, effects: effects);
    }

    public void Update(GameTime gameTime)
    {
        if (!isStomped)
        {
            //TURN GOOMBA AROUND WHEN IT HITS THE EDGE OF ITS PATROL AREA
            if (Position.X <= leftBound || Position.X >= rightBound)
            {
                Velocity = new Vector2(-Velocity.X, Velocity.Y);
            }
        }
        else
        {
            Velocity = new Vector2(0f, Velocity.Y);
        }

        sprite.UpdateAnimation(gameTime);
    }

    public void Stomp()
    {
        if (isStomped)
            return;
        
        sprite.Play("Stomped");
        isStomped = true;
        Velocity = new Vector2(0f, Velocity.Y);
    }

    public void OnCollision(in CollisionEvent collision)
    {
        if (collision.Other is IBlock && collision.Normal.X != 0f)
        {
            Velocity = new Vector2(-collision.RelativeVelocityBefore.X, Velocity.Y);
        }
    }
}
