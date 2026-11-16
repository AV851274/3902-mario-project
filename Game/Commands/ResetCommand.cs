using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public class ResetCommand : ICommand
{
    private readonly IGameControls world;

    public ResetCommand(IGameControls world)
    {
        this.world = world;
    }

    public void Execute()
    {
        world.ResetGame();
    }
}