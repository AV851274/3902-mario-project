using Microsoft.Xna.Framework;
using Game2D.Interfaces;

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