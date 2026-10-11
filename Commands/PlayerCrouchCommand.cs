using Mario.Interfaces.Player;
using Mario.Interfaces;

namespace Mario.Commands;

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
