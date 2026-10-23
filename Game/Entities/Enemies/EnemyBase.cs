using Mario.Game.Interfaces;
using Mario.Game.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public abstract class EnemyBase : IEnemy, IPhysicsBody
{
    // Fish states, can do other enums for other enemies if needed.
    // Not sure if this is the best spot for it, feel free to move it if you know a better place.
    public enum FishColor
    {
        Gray,
        Green,
    }
    protected readonly ISprite sprite;
    private readonly float width;
    private readonly float height;

    protected bool DefeatedByFireball { get; private set; }
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 PendingAcceleration { get; set; }
    public bool IsOnGround { get; set; }
    public float GravityScale
    {
        get
        {
            if (this is CheepCheep && !IsDefeated)
                return 0f;
            else if (this is CheepCheep) return 0.2f;
            return 1f;
        }
    }
    public ICollider.BlockingModes BlockingMode
    {
        get
        {
            if (this is CheepCheep)
                return ICollider.BlockingModes.ThroughWalls;
            return ICollider.BlockingModes.None;
        }
    }
    public bool canBeStomped
    {
        get
        {
            return !(this is CheepCheep);
        }
    }
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
        if (this is CheepCheep) Velocity = new Vector2(0f, -100f);
        else Velocity = new Vector2(0f, -200f);
        IsOnGround = false;
    }

    public void OnCollision(CollisionEvent collision)
    {
        // Only reverse while approaching, even if several blocks notify contact.
        if (collision.Other is IBlock && collision.Normal.X != 0f &&
            Vector2.Dot(collision.RelativeVelocityBefore, collision.Normal) < 0f)
        {
            OnWallBounce(collision.RelativeVelocityBefore.X);
        }
    }

    protected virtual void OnWallBounce(float RelativeVelocityBeforeX) { }
}
