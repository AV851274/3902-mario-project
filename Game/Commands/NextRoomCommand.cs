using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public class NextRoomCommand : ICommand
{
    private MarioGame game;

    public NextRoomCommand(MarioGame game)
    {
        this.game = game;
    }

    public void Execute()
    {
        game.NextRoom();
    }
}
