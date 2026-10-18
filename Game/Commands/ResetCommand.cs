using Mario.Game;
using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

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
