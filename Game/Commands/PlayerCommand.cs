using Mario.Game.Interfaces.Player;
using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public abstract class PlayerCommand : ICommand
{
    protected IPlayer player;

    protected PlayerCommand(IPlayer player)
    {
        this.player = player;
    }

    public abstract void Execute();
}
