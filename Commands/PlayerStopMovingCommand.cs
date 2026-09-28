public class PlayerStopMovingCommand : PlayerCommand
{
    public PlayerStopMovingCommand(IPlayer player) : base(player)
    {
    }

    public override void Execute()
    {
        player.StopMoving();
    }
}