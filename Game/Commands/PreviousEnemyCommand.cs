using Mario.Game.Entities.Enemies;
using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public class PreviousEnemyCommand : ICommand
{
    private EnemyCycler enemies;

    public PreviousEnemyCommand(EnemyCycler enemies)
    {
        this.enemies = enemies;
    }

    public void Execute()
    {
        enemies.Previous();
    }
}
