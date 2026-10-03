using Mario;
using Mario.Interfaces;

namespace Mario.Commands;

public class QuitCommand : ICommand
{
    private MarioGame game;

    public QuitCommand(MarioGame game)
    {
        this.game = game;
    }

    public void Execute()
    {
        game.Exit();
    }
}
