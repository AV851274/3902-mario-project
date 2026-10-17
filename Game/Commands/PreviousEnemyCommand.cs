using Mario.Entities.Enemies;
using Mario.Interfaces;

namespace Mario.Commands;

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
