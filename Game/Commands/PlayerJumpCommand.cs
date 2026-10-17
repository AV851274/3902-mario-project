using Mario.Game.Interfaces.Player;
using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

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
