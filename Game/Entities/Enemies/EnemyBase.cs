using Mario.Game.Interfaces;
using Mario.Game.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public abstract class EnemyBase : IEnemy, IPhysicsBody
{
    protected readonly ISprite sprite;
    private readonly float width;
    private readonly float height;

    protected bool DefeatedByFireball { get; private set; }
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 PendingAcceleration { get; set; }
    public bool IsOnGround { get; set; }
    public float GravityScale => 1f;
    public ICollider.BlockingModes BlockingMode => ICollider.BlockingModes.None;
    public Box Bounds => new Box(Position.X, Position.Y, width, height);
    public bool Active => Position.Y <= 1000f;
    public bool CollisionEnabled => Active && !IsDefeated;
    public virtual bool IsDefeated => DefeatedByFireball;
    public virtual bool CanHurtPlayer => !IsDefeated;

    protected EnemyBase(ISprite sprite, Vector2 position, float width, float height)
    {
        this.sprite = sprite;
        Position = position;
        this.width = width;
        this.height = height;
    }

    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void Stomp();

    public void HitByFireball()
    {
        if (IsDefeated)
            return;

        DefeatedByFireball = true;
        Velocity = new Vector2(0f, -200f);
        IsOnGround = false;
    }

    public void OnCollision(CollisionEvent collision)
    {
        // Only reverse while approaching, even if several blocks notify contact.
        if (collision.Other is IBlock && collision.Normal.X != 0f &&
            Vector2.Dot(collision.RelativeVelocityBefore, collision.Normal) < 0f)
        {
            Velocity = new Vector2(-collision.RelativeVelocityBefore.X, Velocity.Y);
            OnWallBounce();
        }
    }

    protected virtual void OnWallBounce() { }
}
