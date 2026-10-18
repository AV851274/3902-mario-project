using Mario.Game.Interfaces.Player;
using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public class PlayerStopMovingCommand : PlayerCommand
{
    public PlayerStopMovingCommand(IPlayer player) : base(player)
    {
    }

    public override void Execute()
    {
        player.BodyAction.Stop();
    }
}
