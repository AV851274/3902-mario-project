using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public class PreviousRoomCommand : ICommand
{
    private MarioGame game;

    public PreviousRoomCommand(MarioGame game)
    {
        this.game = game;
    }

    public void Execute()
    {
        game.PreviousRoom();
    }
}
