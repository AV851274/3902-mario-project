using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public class ToggleHitboxesCommand : ICommand
{
    private readonly IGameControls world;

    public ToggleHitboxesCommand(IGameControls world)
    {
        this.world = world;
    }

    public void Execute()
    {
        world.ToggleHitboxes();
    }
}