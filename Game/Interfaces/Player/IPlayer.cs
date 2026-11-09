using System;
using Mario.Game.Audio;
using Mario.Game.Entities.Players;
using Microsoft.Xna.Framework;

namespace Mario.Game.Interfaces.Player;

public interface IPlayer : IGameObject, IPhysicsBody
{
    IPlayerBodyAction BodyAction { get; }
    IMarioPowerState MarioPowerState { get; }
    event Action<SoundType> SoundRequested;
    event Action DeathStarted;
    event Action Died;
    event Action FlagpoleReached;
    void Attack();
    void Crouch();
    void StartFlagpoleSlide(IBlock flagpole);
    void TakeDamage();
    void SetPower(MarioPower power);
    void SetSwimming(bool swimming);
    void UpdateAnimation(GameTime gameTime);
    event Action WarpRequested;
}
