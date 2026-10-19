using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Interfaces;

public interface IEnemy : IGameObject, ICollider
{
    Vector2 Position { get; }

    // Defeated enemies are ignored by collisions (e.g. a stomped Goomba falling off screen)
    bool IsDefeated { get; }

    // Whether touching this enemy from the side damages Mario.Game.Bootstrap (a resting Koopa shell doesn't)
    bool CanHurtPlayer { get; }

    void Stomp();
    void HitByFireball();
}
