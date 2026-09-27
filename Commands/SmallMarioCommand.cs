public class SmallMarioCommand : ICommand
{
    private IPlayer player;

    public SmallMarioCommand(IPlayer player)
    {
        this.player = player;
    }

    public void Execute()
    {
        player.BecomeSmall();
    }
}
