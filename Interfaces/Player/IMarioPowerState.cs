using Mario.Players;
using Microsoft.Xna.Framework;

namespace Mario.Interfaces.Player;

public interface IMarioPowerState
{
    bool CanShootFireball { get; }
    Vector2 DrawOffset { get; }
    MarioPower PowerState { get; }
    MarioPower TakeDamage();
}