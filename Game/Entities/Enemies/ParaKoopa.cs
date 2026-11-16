using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public class ParaKoopa : EnemyBase
{
    private readonly IEntitySpawner spawner;
    private readonly ISpriteFactory sprites;

    private Direction facingDirection = Direction.Left;

    public Direction FacingDirection => facingDirection;

    private bool transformed;
    public override bool Active => !transformed && base.Active;
    public override bool IsDefeated => transformed || base.IsDefeated;

    public override bool CanHurtPlayer => !IsDefeated;

    public ParaKoopa(ISprite sprite, Vector2 initialPosition, IEntitySpawner spawner, ISpriteFactory sprites)
        : base(sprite, initialPosition, GameConstants.Entities.StandardSize,
            GameConstants.Entities.TallHeight)
    {
        this.spawner = spawner;
        this.sprites = sprites;
        Velocity = new Vector2(-GameConstants.Enemies.StandardWalkSpeed, 0f);
        IsOnGround = true;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!Active)
            return;

        SpriteEffects effects = facingDirection == Direction.Right
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;

        sprite.Draw(spriteBatch, Position, effects: effects);
    }

    public override void Update(GameTime gameTime)
    {
        if (IsOnGround)
        {
            Velocity = new Vector2(Velocity.X, GameConstants.Enemies.StandardJumpVelocity);
            IsOnGround = false;
        }

        sprite.UpdateAnimation(gameTime);
    }

    public override void HitByFireball()
    {
        Stomp();
    }

    public override void Stomp()
    {
        if (!Active)
            return;

        transformed = true;
        spawner.Spawn(new Turtle(sprites.Create("enemies", "turtle"), Position)
        {
            Velocity = Velocity,
        });
    }

    protected override void OnWallBounce(float RelativeVelocityBeforeX)
    {
        //-collision.RelativeVelocityBefore.X
        Velocity = new Vector2(-RelativeVelocityBeforeX, Velocity.Y);
        facingDirection = Velocity.X < 0f ? Direction.Left : Direction.Right;
    }
}
