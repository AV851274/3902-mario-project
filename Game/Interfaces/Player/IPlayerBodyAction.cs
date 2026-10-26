using Mario.Game.Animation;
using Microsoft.Xna.Framework;

namespace Mario.Game.Interfaces.Player;

public interface IPlayerBodyAction
{
    public void Move(Direction direction);
    public void Stop();
    public void Jump();
    public void SetJumpHeld(bool isHeld);
    public void Dash(Direction direction);
    void Crouch();
    void Update(GameTime gameTime);
}
