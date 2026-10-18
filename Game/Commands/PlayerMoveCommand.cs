using Mario.Game.Interfaces.Player;
using Mario.Game.Animation;
using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public class PlayerMoveCommand : PlayerCommand
{
    private Direction direction;

    public PlayerMoveCommand(IPlayer player, Direction direction) : base(player)
    {
        this.direction = direction;
    }

    public override void Execute()
    {
        player.BodyAction.Move(direction);
    }
}
