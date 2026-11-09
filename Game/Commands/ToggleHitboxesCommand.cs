using Mario.Game.Interfaces;
using Mario.Game.State;

namespace Mario.Game.Commands;

public class ToggleHitboxesCommand : ICommand
{
    private GameWorld world;

    public ToggleHitboxesCommand(GameWorld world)
    {
        this.world = world;
    }

    public void Execute()
    {
        world.ToggleHitboxes();
    }
}
