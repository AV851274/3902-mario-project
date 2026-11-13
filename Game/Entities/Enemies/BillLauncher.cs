using System;
using Mario.Game.Animation;
using Mario.Game.Entities.Players;
using Mario.Game.Interfaces;
using Mario.Game.Entities.Projectiles;
using Mario.Game.Interfaces.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public class BillLauncher : StaticEnemyBase, IPlayerFollower
{
    private readonly IEntitySpawner spawner;

    private const int Width = 40;
    private const int Height = 40;
    private const float ShootCooldown = 3f;
    private const float MinShootDistance = 72f;
    private IPlayer target;
    private float cooldownTimer = ShootCooldown;

    public Direction FacingDirection =>
        target is not null && target.Position.X < Position.X ? Direction.Left : Direction.Right;

    public override bool CanHurtPlayer => false;


    public void Follow(IPlayer player)
    {
        target = player;
    }

    public BillLauncher(ISprite sprite, Vector2 initialPosition, IEntitySpawner spawner)
        : base(sprite, initialPosition, Width, Height)
    {
        this.spawner = spawner;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        return;
    }

    public override void Update(GameTime gameTime)
    {
        if (target is null)
            return;

        cooldownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Don't fire when Mario is standing right next to the launcher, like the original
        if (cooldownTimer <= 0f && Math.Abs(target.Position.X - Position.X) > MinShootDistance)
        {
            ShootBill();
            cooldownTimer = ShootCooldown;
        }
    }

    public void ShootBill()
    {
        float direction = FacingDirection == Direction.Left ? -1f : 1f;
        spawner.TrySpawnProjectile(ProjectileType.BulletBill,
            Position, new Vector2(250f * direction, 0f));
    }

    public override void Stomp()
    {
        return;
    }

    public override void HitByFireball()
    {
        return;
    }
}
