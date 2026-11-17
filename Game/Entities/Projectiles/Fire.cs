using Mario.Game.Physics;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Game.Interfaces.Player;

namespace Mario.Game.Entities.Projectiles;

public class Fire : IProjectile
{
    private const int BoundsWidth = GameConstants.World.TileSize + GameConstants.World.HalfTileSize;
    private const int BoundsHeight = GameConstants.World.HalfTileSize;
    private const int GridAdjustment = 1;
    private ISprite sprite;
    private float lifeTime;
    private float elapsedTime = 0f;

    public Vector2 PendingAcceleration { get; set; }
    public float GravityScale => 0f;
    public bool IsOnGround { get; set; }
    public bool CollisionEnabled => true;
    public Vector2 Position { get; set; }
    public ICollider.BlockingModes BlockingMode => ICollider.BlockingModes.ThroughWalls;
    public Box Bounds => new Box(Position.X, Position.Y, BoundsWidth, BoundsHeight);
    public bool Active { get; private set; }
    public Vector2 Velocity { get; set; }

    public Fire(ISprite sprite, Vector2 position, Vector2 velocity, float lifeTime)
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
        if ((int)Position.Y % GameConstants.World.TileSize != 0)
        {
            Position = new Vector2(Position.X, Position.Y + GridAdjustment);
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
