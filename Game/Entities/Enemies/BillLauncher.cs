using System;
using Mario.Game.Animation;
using Mario.Game.Entities.Players;
using Mario.Game.Interfaces;
using Mario.Game.Interfaces.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public class BillLauncher : StaticEnemyBase
{
    private const int Width = 40;
    private const int Height = 40;
    private IPlayer target;

    public Direction FacingDirection =>
        target is not null && target.Position.X < Position.X ? Direction.Left : Direction.Right;

    public override bool CanHurtPlayer => false;

    public event Action<Vector2, Direction> SummonBill;

    public void Follow(IPlayer player)
    {
        target = player;
    }

    public BillLauncher(ISprite sprite, Vector2 initialPosition)
        : base(sprite, initialPosition, Width, Height)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        return;
    }

    public override void Update(GameTime gameTime)
    {
        if (target is null)
            return;

        if (Math.Abs(target.Position.X - Position.X) > 72) {
            if (Random.Shared.NextDouble() < 0.01) // 1% chance to shoot each frame
            {
                ShootBill();
            } 
        }
        
    }

    public void ShootBill()
    {
        SummonBill?.Invoke(Position, FacingDirection);
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