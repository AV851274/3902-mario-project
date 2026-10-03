using Mario.Animation;
using Mario.Interfaces;

namespace Mario.Commands;

public class PlayerDashCommand : PlayerCommand
{
    private Direction direction;

    public PlayerDashCommand(IPlayer player, Direction direction) : base(player)
    {
        this.direction = direction;
    }

    public override void Execute()
    {
        if (direction == Direction.Left)
            player.DashLeft();
        else
            player.DashRight();
    }
}
