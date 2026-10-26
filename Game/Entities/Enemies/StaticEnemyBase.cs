using Mario.Game.Interfaces;
using Mario.Game.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public abstract class StaticEnemyBase : IEnemy
{
    protected readonly ISprite sprite;
    private readonly float width;
    private readonly float height;

    protected bool DefeatedByFireball { get; private set; }
    public Vector2 Position { get; set; }
    public virtual ICollider.BlockingModes BlockingMode => ICollider.BlockingModes.None;
    public virtual bool canBeStomped => true;
    public Box Bounds => new Box(Position.X, Position.Y, width, height);
    public bool Active => Position.Y <= 1000f;
    public bool CollisionEnabled => Active && !IsDefeated;
    public virtual bool IsDefeated => DefeatedByFireball;
    public virtual bool CanHurtPlayer => !IsDefeated;

    protected StaticEnemyBase(ISprite sprite, Vector2 position, float width, float height)
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
    }
}

