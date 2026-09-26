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
