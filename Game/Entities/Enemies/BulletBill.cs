using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public class BulletBill : EnemyBase
{
    private const float MaxFlightTime = 4f;

    private bool isStomped;
    private float flightTime;
    public override float GravityScale => IsDefeated ? 1f : 0f;
    public override bool IsDefeated => isStomped;
    public override ICollider.BlockingModes BlockingMode => ICollider.BlockingModes.ThroughWalls;
    public bool IsSpawning => flightTime < SpawningDuration;

    private const float SpawningDuration = 0.5f;

    public BulletBill(ISprite sprite, Vector2 initialPosition, Vector2 velocity)
        : base(sprite, initialPosition, GameConstants.Entities.StandardSize,
            GameConstants.Entities.StandardSize)
    {
        Velocity = velocity;
        Position = initialPosition;
    }

    public override void HitByFireball()
    {
        return;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Direction facingDirection = Velocity.X < 0f ? Direction.Left : Direction.Right;
        SpriteEffects effects = facingDirection == Direction.Left
            ? SpriteEffects.None
            : SpriteEffects.FlipHorizontally;
        if (IsDefeated)
            effects |= SpriteEffects.FlipVertically;

        sprite.Draw(spriteBatch, Position, effects: effects);
    }

    public override void Update(GameTime gameTime)
    {
        if (IsDefeated)
        {
            Velocity = new Vector2(0f, Velocity.Y);
        }
        else
        {
            flightTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (flightTime >= MaxFlightTime)
            {
                Stomp();
            }
        }

        sprite.UpdateAnimation(gameTime);
    }

    public override void Stomp()
    {
        if (DefeatedByFireball)
            return;

        isStomped = true;
        Velocity = Vector2.Zero;

    }
}