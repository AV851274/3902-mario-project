using Mario.Players;
using Microsoft.Xna.Framework;

namespace Mario.Interfaces.Player;

public interface IMarioPowerState
{
    bool CanShootFireball { get; }
    bool CanCrouch { get; }
    MarioPower Power { get; }
    MarioPower TakeDamage();
}