using Microsoft.Xna.Framework.Content;

namespace Mario.Game.Rendering.Sprites;

public class SpriteFactory
{
    public MarioSpriteFactory Mario { get; private set; }
    public EnemySpriteFactory Enemies { get; private set; }
    public ItemSpriteFactory Items { get; private set; }
    public BlockSpriteFactory Blocks { get; private set; }
    public BackgroundSpriteFactory Background { get; private set; }

    public void LoadTextures(ContentManager content)
    {
        Mario = new MarioSpriteFactory(content);
        Enemies = new EnemySpriteFactory(content);
        Items = new ItemSpriteFactory(content);
        Blocks = new BlockSpriteFactory(content);
        Background = new BackgroundSpriteFactory(content);
    }
}
