using Mario.Game.Entities.Players;
using Microsoft.Xna.Framework;

namespace Mario.Game.Interfaces.Player;

public interface IMarioPowerState
{
    bool CanShootFireball { get; }
    bool CanCrouch { get; }
    MarioPower Power { get; }
    MarioPower TakeDamage();
}