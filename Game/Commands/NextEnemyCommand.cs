using Mario.Entities.Enemies;
using Mario.Interfaces;

namespace Mario.Commands;

public class NextEnemyCommand : ICommand
{
    private EnemyCycler enemies;

    public NextEnemyCommand(EnemyCycler enemies)
    {
        this.enemies = enemies;
    }

    public void Execute()
    {
        enemies.Next();
    }
}
