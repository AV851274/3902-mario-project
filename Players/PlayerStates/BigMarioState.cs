using Microsoft.Xna.Framework;
using Game2D.Interfaces;

public class BigMarioState : IMarioState
{
    public ISprite Sprite { get; }
    public bool CanShootFireball => false;
    public Vector2 DrawOffset => new Vector2(0, -40);

    public BigMarioState(SpriteFactory spriteFactory)
    {
        Sprite = spriteFactory.CreateBigMarioSprite();
    }
}