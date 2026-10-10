using Mario.Animation;
using Mario.Players;

namespace Mario.Interfaces.Player;

public interface IPlayerAction
{
    public void Move(Direction direction);
    public void Stop();
    public void Jump();
    public void Dash(Direction direction);
    void Crouch();
    void Attack();
    void TakeDamage();
}