using Mario.Interfaces;

namespace Mario.Commands;

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
