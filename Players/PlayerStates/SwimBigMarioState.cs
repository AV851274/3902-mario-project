using Microsoft.Xna.Framework;
using Game2D.Interfaces;

public class SwimBigMarioState : IMarioState
{
    public ISprite Sprite { get; }
    public bool CanShootFireball => false;
    public Vector2 DrawOffset => new Vector2(0, -40);
    public MarioPower marioPower => MarioPower.Big;

    public SwimBigMarioState(SpriteFactory spriteFactory)
    {
        Sprite = spriteFactory.CreateBigSwimmingMarioSprite();
    }
}