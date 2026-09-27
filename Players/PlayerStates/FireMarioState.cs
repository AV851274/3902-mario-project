using Microsoft.Xna.Framework;
using Game2D.Interfaces;

public class FireMarioState : IMarioState
{
    public ISprite Sprite { get; }
    public bool CanShootFireball => true;
    public Vector2 DrawOffset => new Vector2(0, -40);

    public FireMarioState(SpriteFactory spriteFactory)
    {
        Sprite = spriteFactory.CreateFireMarioSprite();
    }
}