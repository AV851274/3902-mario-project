using Mario.Game.Interfaces.Player;

namespace Mario.Game.Interfaces;

public interface IPlayerFollower
{
    void Follow(IPlayer player);
}