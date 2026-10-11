using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Interfaces;

public interface IEnemy : IGameObject
{
    Vector2 Position { get; }

    void Stomp();

    //TODO: Collision next sprint (hit by fireball, shell, etc.)
}