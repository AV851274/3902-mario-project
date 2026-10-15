using Mario.Interfaces.Player;

namespace Mario.Players.PlayerStates;

public class FireMarioState : IMarioPowerState
{
    public bool CanShootFireball => true;
    public bool CanCrouch => true;
    public MarioPower Power => MarioPower.Fire;

    public MarioPower TakeDamage() => MarioPower.Big;
}
