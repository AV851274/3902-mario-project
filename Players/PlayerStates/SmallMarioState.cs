using Microsoft.Xna.Framework;
using Game2D.Interfaces;

public class SmallMarioState : IMarioState
{
    public ISprite Sprite { get; }
    public bool CanShootFireball => false;
    public Vector2 DrawOffset => Vector2.Zero;

    public SmallMarioState(SpriteFactory spriteFactory)
    {
        Sprite = spriteFactory.CreateSmallMarioSprite();
    }
}