using Mario.Animation;
using Mario.Interfaces;

namespace Mario.Commands;

public class PlayerMoveCommand : PlayerCommand
{
    private Direction direction;

    public PlayerMoveCommand(IPlayer player, Direction direction) : base(player)
    {
        this.direction = direction;
    }

    public override void Execute()
    {
        if (direction == Direction.Left)
            player.MoveLeft();
        else
            player.MoveRight();
    }
}
