using Mario.Game.Interfaces.Player;
using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public class PlayerAttackCommand : PlayerCommand
{
    public PlayerAttackCommand(IPlayer player) : base(player)
    {
    }

    public override void Execute()
    {
        player.Attack();
    }
}
