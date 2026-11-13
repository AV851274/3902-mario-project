using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public class PreviousRoomCommand : ICommand
{
    private readonly IGameControls world;

    public PreviousRoomCommand(IGameControls world)
    {
        this.world = world;
    }

    public void Execute()
    {
        world.PreviousRoom();
    }
}