using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Interfaces;

public interface IEnemy : IGameObject
{
    Vector2 Position { get; }
    Rectangle Bounds { get; }

    // Defeated enemies are ignored by collisions (e.g. a stomped Goomba falling off screen)
    bool IsDefeated { get; }

    // Whether touching this enemy from the side damages Mario.Bootstrap (a resting Koopa shell doesn't)
    bool CanHurtPlayer { get; }

    void Stomp();
    void LandOn(float groundTop);
    void BounceOffWall(float newX);
}
