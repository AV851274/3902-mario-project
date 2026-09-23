using Microsoft.Xna.Framework;

public interface IPlayer
{
    Vector2 Position { get; }
    bool IsMoving { get; }
    int FacingDirection { get; }
    bool IsOnGround { get; }
    void MoveLeft();
    void MoveRight();
    void StopMoving();
    void Jump();
    void Dash();
    void Update(GameTime gameTime);
}