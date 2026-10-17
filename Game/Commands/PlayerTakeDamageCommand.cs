using Mario.Game.Interfaces.Player;
using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public class PlayerTakeDamageCommand : PlayerCommand
{
    public PlayerTakeDamageCommand(IPlayer player) : base(player)
    {
    }

    public override void Execute()
    {
        player.TakeDamage();
    }
}
