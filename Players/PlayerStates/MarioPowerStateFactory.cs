using System;
using Mario.Interfaces.Player;

namespace Mario.Players.PlayerStates;

public static class MarioPowerStateFactory
{
    public static IMarioPowerState Create(MarioPower power)
        => power switch {
            MarioPower.Small => new SmallMarioState(),
            MarioPower.Big => new BigMarioState(),
            MarioPower.Fire => new FireMarioState(),
            _ => throw new ArgumentOutOfRangeException(nameof(power))
        };
}