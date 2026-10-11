using Mario.Interfaces.Player;
using Mario.Interfaces;

namespace Mario.Commands;

public class PlayerJumpCommand : PlayerCommand
{
    public PlayerJumpCommand(IPlayer player) : base(player)
    {
    }

    public override void Execute()
    {
        player.BodyAction.Jump();
    }
}
