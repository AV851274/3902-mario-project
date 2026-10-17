using Mario.Game;
using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

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
