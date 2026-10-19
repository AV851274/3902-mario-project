using System;
using Mario.Game.Animation;
using Mario.Game.Entities.Players;
using Microsoft.Xna.Framework;

namespace Mario.Game.Interfaces.Player;

public interface IPlayer : IGameObject
{
    IPlayerBodyAction BodyAction { get; }
    IMarioPowerState MarioPowerState { get; }
    Vector2 Position { get; }
    Rectangle Bounds { get; }
    Vector2 Velocity { get; set; }
    bool IsOnGround { get; set; }
    void SetPosition(Vector2 position);
    event Action<Vector2, Direction> SummonFireball;
    event Action Died;
    void Attack();
    void Crouch();
    void TakeDamage();
    void SetPower(MarioPower power);
    void SetSwimming(bool swimming);
    void UpdateAnimation(GameTime gameTime);
}
