public class FireMarioCommand : ICommand
{
    private IPlayer player;

    public FireMarioCommand(IPlayer player)
    {
        this.player = player;
    }

    public void Execute()
    {
        player.BecomeFire();
    }
}