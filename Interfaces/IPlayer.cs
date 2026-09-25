using Microsoft.Xna.Framework;
using Game2D.Animation;

public interface IPlayer
{
    Vector2 Position { get; }
    bool IsMoving { get; }
    Direction FacingDirection { get; }
    bool IsOnGround { get; }
    void MoveLeft();
    void MoveRight();
    void StopMoving();
    void Jump();
    void Dash();
    void DashLeft();
    void DashRight();
    void Update(GameTime gameTime);
}