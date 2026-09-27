using Microsoft.Xna.Framework;
using Game2D.Animation;
using Microsoft.Xna.Framework.Graphics;

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
    void Attack();
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}