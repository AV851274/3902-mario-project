using Mario;
using Mario.Interfaces;

namespace Mario.Commands;

public class ResetCommand : ICommand
{
    private MarioGame game;

    public ResetCommand(MarioGame game)
    {
        this.game = game;
    }

    public void Execute()
    {
        game.ResetGame();
    }
}
