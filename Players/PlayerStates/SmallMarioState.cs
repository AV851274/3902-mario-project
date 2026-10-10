using Mario.Interfaces.Player;

namespace Mario.Players.PlayerStates;

public class SmallMarioState : IMarioPowerState
{
    public bool CanShootFireball => false;
    public bool CanCrouch => false;
    public MarioPower Power => MarioPower.Small;

    public MarioPower TakeDamage() => MarioPower.Dead;
}
