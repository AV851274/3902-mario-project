using Mario.Game.Entities.Enemies;
using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

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
