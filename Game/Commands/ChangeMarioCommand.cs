using Mario.Game.Interfaces;
using Mario.Game.State;
using Mario.Game.Entities.Players;

namespace Mario.Game.Commands;

public class ChangeMarioCommand : ICommand
{
    private GameWorld world;
    private bool swimming;
    private MarioPower power;

    public ChangeMarioCommand(GameWorld world, bool swimming, MarioPower power)
    {
        this.world = world;
        this.swimming = swimming;
        this.power = power;
    }

    public void Execute()
    {
        world.ChangeMario(swimming, power);
    }
}
