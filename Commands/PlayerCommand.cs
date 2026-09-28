public abstract class PlayerCommand : ICommand
{
    protected IPlayer player;

    public PlayerCommand(IPlayer player)
    {
        this.player = player;
    }

    public abstract void Execute();
}