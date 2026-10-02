using Mario;
using Mario.Interfaces;
using Mario.Players;

namespace Mario.Commands;

public class ChangeMarioCommand : ICommand
{
    private MarioGame game;
    private bool swimming;
    private MarioPower power;

    public ChangeMarioCommand(MarioGame game, bool swimming, MarioPower power)
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
