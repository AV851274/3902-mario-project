using Mario.Game.Interfaces.Player;
using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public class PlayerCrouchCommand : PlayerCommand
{
    public PlayerCrouchCommand(IPlayer player) : base(player)
    {
    }

    public override void Execute()
    {
        player.Crouch();
    }
}
