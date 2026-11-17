using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public class Goomba : EnemyBase
{
    private bool isStomped;

    public override bool IsDefeated => isStomped || base.IsDefeated;

    public Goomba(ISprite sprite, Vector2 initialPosition)
        : base(sprite, initialPosition, GameConstants.Entities.StandardSize,
            GameConstants.Entities.StandardSize)
    {
        Velocity = new Vector2(-GameConstants.Enemies.StandardWalkSpeed, 0f);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = IsDefeated ? SpriteEffects.FlipVertically : SpriteEffects.None;
        sprite.Draw(spriteBatch, Position, effects: effects);
    }

    public override void Update(GameTime gameTime)
    {
        if (IsDefeated)
        {
            Velocity = new Vector2(0f, Velocity.Y);
        }

        sprite.UpdateAnimation(gameTime);
    }

    public override void Stomp()
    {
        if (DefeatedByFireball)
            return;

        if (isStomped)
            return;

        sprite.Play("Stomped");
        isStomped = true;
        Velocity = new Vector2(0f, Velocity.Y);
    }

    protected override void OnWallBounce(float RelativeVelocityBeforeX)
    {
        Velocity = new Vector2(-RelativeVelocityBeforeX, Velocity.Y);
    }
}