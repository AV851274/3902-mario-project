using Mario.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mario.Interfaces;

public interface IPlayer
{
    event Action<Vector2, Direction> SummonFireball;
    Vector2 Position { get; }
    bool IsMoving { get; }
    Direction FacingDirection { get; }
    bool IsOnGround { get; }
    void MoveLeft();
    void MoveRight();
    void StopMoving();
    void Jump();
    void Crouch();
    void Dash();
    void DashLeft();
    void DashRight();
    void Attack();
    void BecomeSmall();
    void BecomeBig();
    void BecomeFire();
    void TakeDamage();
    void Draw(SpriteBatch spriteBatch);
    void Update(GameTime gameTime);
}
