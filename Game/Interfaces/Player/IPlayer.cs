using System;
using Mario.Game.Animation;
using Mario.Game.Entities.Players;
using Microsoft.Xna.Framework;

namespace Mario.Game.Interfaces.Player;

public interface IPlayer : IGameObject, IPhysicsBody
{
    IPlayerBodyAction BodyAction { get; }
    IMarioPowerState MarioPowerState { get; }
    event Action<Vector2, Direction> SummonFireball;
    event Action Died;
    void Attack();
    void Crouch();
    void TakeDamage();
    void SetPower(MarioPower power);
    void SetSwimming(bool swimming);
    void UpdateAnimation(GameTime gameTime);
}
