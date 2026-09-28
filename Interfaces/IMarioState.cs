using Microsoft.Xna.Framework;
using Game2D.Interfaces;

public interface IMarioState
{
    ISprite Sprite { get; }
    bool CanShootFireball { get; }
    Vector2 DrawOffset { get; }
}