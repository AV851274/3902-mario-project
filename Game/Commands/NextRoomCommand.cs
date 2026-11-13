using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public class NextRoomCommand : ICommand
{
    private readonly IGameControls world;

    public NextRoomCommand(IGameControls world)
    {
        this.world = world;
    }

    public void Execute()
    {
        world.NextRoom();
    }
}