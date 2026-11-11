using Mario.Game.Interfaces;
using Mario.Game.State;

namespace Mario.Game.Commands;

public class ToggleMusicCommand : ICommand
{
    private GameWorld world;

    public ToggleMusicCommand(GameWorld world)
    {
        this.world = world;
    }

    public void Execute()
    {
        world.ToggleMusicMute();
    }
}
