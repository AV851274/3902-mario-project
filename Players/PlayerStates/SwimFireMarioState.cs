using Mario.Interfaces;
using Mario.Sprites;
using Microsoft.Xna.Framework;

namespace Mario.Players.PlayerStates;

public class SwimFireMarioState : IMarioState
{
    public ISprite Sprite { get; }
    public bool CanShootFireball => true;
    public Vector2 DrawOffset => new Vector2(0, -40);
    public MarioPower marioPower => MarioPower.Fire;

    public SwimFireMarioState(SpriteFactory spriteFactory)
    {
        Sprite = spriteFactory.CreateFireSwimmingMarioSprite();
    }
}
