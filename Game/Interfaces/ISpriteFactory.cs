namespace Mario.Game.Interfaces;

public interface ISpriteFactory
{
    ISprite Create(string category, string id);
}
