using Mario.Interfaces.Player;
using Mario.Interfaces;

namespace Mario.Commands;

public abstract class PlayerCommand : ICommand
{
    protected IPlayer player;

    protected PlayerCommand(IPlayer player)
    {
        this.player = player;
    }

    public abstract void Execute();
}
