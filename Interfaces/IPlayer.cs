using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    void Attack();
    void BecomeSmall();
    void BecomeBig();
    void BecomeFire();
    void Draw(SpriteBatch spriteBatch);
    void Update(GameTime gameTime);
}