using Mario.Game.Interfaces;
using Mario.Game.State;

namespace Mario.Game.Commands;

public class PreviousRoomCommand : ICommand
{
    private GameWorld world;

    public PreviousRoomCommand(GameWorld world)
    {
        this.world = world;
    }

    public void Execute()
    {
        world.PreviousRoom();
    }
}
