using System;
using Mario.Animation;
using Mario.Players;
using Microsoft.Xna.Framework;

namespace Mario.Interfaces.Player;

public interface IPlayer : IGameObject
{
    IPlayerBodyAction BodyAction { get; }
    IMarioPowerState MarioPowerState { get; }
    Vector2 Position { get; }
    event Action<Vector2, Direction> SummonFireball;
    void Attack();
    void Crouch();
    void TakeDamage();
    void SetPower(MarioPower power);
    void SetSwimming(bool swimming);
}
