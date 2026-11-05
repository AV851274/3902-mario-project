using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public class ToggleHitboxesCommand : ICommand
{
    private MarioGame game;

    public ToggleHitboxesCommand(MarioGame game)
    {
        this.game = game;
    }

    public void Execute()
    {
        game.ToggleHitboxes();
    }
}
