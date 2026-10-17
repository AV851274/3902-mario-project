using Mario.Game.Interfaces.Player;

namespace Mario.Game.Entities.Players.PlayerStates;

public class SmallMarioState : IMarioPowerState
{
    public bool CanShootFireball => false;
    public bool CanCrouch => false;
    public MarioPower Power => MarioPower.Small;

    public MarioPower TakeDamage() => MarioPower.Dead;
}
