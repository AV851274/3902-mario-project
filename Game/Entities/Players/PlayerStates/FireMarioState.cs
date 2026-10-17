using Mario.Game.Interfaces.Player;

namespace Mario.Game.Entities.Players.PlayerStates;

public class FireMarioState : IMarioPowerState
{
    public bool CanShootFireball => true;
    public bool CanCrouch => true;
    public MarioPower Power => MarioPower.Fire;

    public MarioPower TakeDamage() => MarioPower.Big;
}
