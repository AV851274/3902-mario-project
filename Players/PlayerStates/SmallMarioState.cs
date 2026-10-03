using Mario.Interfaces;
using Mario.Sprites;
using Microsoft.Xna.Framework;

namespace Mario.Players.PlayerStates;

public class SmallMarioState : IMarioState
{
    public ISprite Sprite { get; }
    public bool CanShootFireball => false;
    public Vector2 DrawOffset => Vector2.Zero;
    public MarioPower marioPower => MarioPower.Small;

    public SmallMarioState(SpriteFactory spriteFactory)
    {
        Sprite = spriteFactory.CreateSmallMarioSprite();
    }
}
