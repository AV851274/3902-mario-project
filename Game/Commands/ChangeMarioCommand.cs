using Mario.Game.Entities.Players;
using Mario.Game.Interfaces.Player;

namespace Mario.Game.Commands;

public class ChangeMarioCommand : PlayerCommand
{
    private readonly bool swimming;
    private readonly MarioPower power;

    public ChangeMarioCommand(
        IPlayer player,
        bool swimming,
        MarioPower power) : base(player)
    {
        this.swimming = swimming;
        this.power = power;
    }

    public override void Execute()
    {
        player.SetSwimming(swimming);
        player.SetPower(power);
    }
}