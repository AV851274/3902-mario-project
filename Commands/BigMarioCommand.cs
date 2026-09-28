public class BigMarioCommand : ICommand
{
    private IPlayer player;

    public BigMarioCommand(IPlayer player)
    {
        this.player = player;
    }

    public void Execute()
    {
        player.BecomeBig();
    }
}
