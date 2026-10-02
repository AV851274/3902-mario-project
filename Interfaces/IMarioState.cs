using Mario.Players;
using Microsoft.Xna.Framework;

namespace Mario.Interfaces;

public interface IMarioState
{
    ISprite Sprite { get; }
    bool CanShootFireball { get; }
    Vector2 DrawOffset { get; }
    MarioPower marioPower { get; }
}
