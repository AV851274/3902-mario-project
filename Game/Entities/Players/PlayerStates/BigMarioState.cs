using Mario.Game.Interfaces.Player;

namespace Mario.Game.Entities.Players.PlayerStates;

public class BigMarioState : IMarioPowerState
{
    public bool CanShootFireball => false;
    public bool CanCrouch => true;
    public MarioPower Power => MarioPower.Big;

    public MarioPower TakeDamage() => MarioPower.Small;
}
