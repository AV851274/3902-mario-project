using Mario.Interfaces;
using Mario.Sprites;
using Microsoft.Xna.Framework;

namespace Mario.Players.PlayerStates;

public class SwimSmallMarioState : IMarioState
{
    public ISprite Sprite { get; }
    public bool CanShootFireball => false;
    public Vector2 DrawOffset => Vector2.Zero;
    public MarioPower marioPower => MarioPower.Small;

    public SwimSmallMarioState(SpriteFactory spriteFactory)
    {
        Sprite = spriteFactory.CreateSmallSwimmingMarioSprite();
    }
}
