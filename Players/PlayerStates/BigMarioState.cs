using Mario.Interfaces;
using Mario.Sprites;
using Microsoft.Xna.Framework;

namespace Mario.Players.PlayerStates;

public class BigMarioState : IMarioState
{
    public ISprite Sprite { get; }
    public bool CanShootFireball => false;
    public Vector2 DrawOffset => new Vector2(0, -40);
    public MarioPower marioPower => MarioPower.Big;

    public BigMarioState(SpriteFactory spriteFactory)
    {
        Sprite = spriteFactory.CreateBigMarioSprite();
    }
}
