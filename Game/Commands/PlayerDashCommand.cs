using Mario.Game.Interfaces.Player;
using Mario.Game.Animation;
using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

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
