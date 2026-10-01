public class PlayerCrouchCommand : PlayerCommand
{
    public PlayerCrouchCommand(IPlayer player) : base(player)
    {
    }

    public override void Execute()
    {
        player.Crouch();
    }
}
