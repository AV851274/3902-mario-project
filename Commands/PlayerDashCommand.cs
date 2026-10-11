using Mario.Interfaces.Player;
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
        player.BodyAction.Dash(direction);
    }
}
