public class PlayerTakeDamageCommand : PlayerCommand
{
    public PlayerTakeDamageCommand(IPlayer player) : base(player)
    {
    }

    public override void Execute()
    {
        player.TakeDamage();
    }
}