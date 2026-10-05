using Mario.Enemies;
using Mario.Interfaces;

namespace Mario.Commands;

public class StompEnemyCommand : ICommand
{
    private readonly EnemyCycler enemies;

    public StompEnemyCommand(EnemyCycler enemies)
    {
        this.enemies = enemies;
    }

    public void Execute()
    {
        enemies.StompCurrent();
    }
}
