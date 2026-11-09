using Mario.Game.Interfaces;
using Mario.Game.State;

namespace Mario.Game.Commands;

public class NextRoomCommand : ICommand
{
    private GameWorld world;

    public NextRoomCommand(GameWorld world)
    {
        this.world = world;
    }

    public void Execute()
    {
        world.NextRoom();
    }
}
