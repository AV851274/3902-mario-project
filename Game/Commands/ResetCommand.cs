using Mario.Game.Interfaces;
using Mario.Game.State;

namespace Mario.Game.Commands;

public class ResetCommand : ICommand
{
    private GameWorld world;

    public ResetCommand(GameWorld world)
    {
        this.world = world;
    }

    public void Execute()
    {
        world.ResetGame();
    }
}
