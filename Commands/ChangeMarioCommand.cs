using Mario;

public class ChangeMarioCommand : ICommand
{
    private Game1 game;
    private bool swimming;
    private MarioPower power;

    public ChangeMarioCommand(Game1 game, bool swimming, MarioPower power)
    {
        this.game = game;
        this.swimming = swimming;
        this.power = power;
    }

    public void Execute()
    {
        game.ChangeMario(swimming, power);
    }
}
