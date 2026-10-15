using Mario.Interfaces.Player;

namespace Mario.Players.PlayerStates;

public class BigMarioState : IMarioPowerState
{
    public bool CanShootFireball => false;
    public bool CanCrouch => true;
    public MarioPower Power => MarioPower.Big;

    public MarioPower TakeDamage() => MarioPower.Small;
}
