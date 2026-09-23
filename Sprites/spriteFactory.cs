using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class SpriteFactory
{
    private Texture2D marioTexture;
    private Texture2D itemTexture;
    private Texture2D blockTexture;

    public void LoadTextures(ContentManager content)
    {
        marioTexture = content.Load<Texture2D>("mario");
        itemTexture = content.Load<Texture2D>("itemsAndBlocks");
        blockTexture = content.Load<Texture2D>("itemsAndBlocks");
    }

    public List<ISprite> CreateItemSprites()
    {
        return new List<ISprite>
        {
            new StaticSprite(itemTexture, new Rectangle(0, 8, 16, 16)),    // mushroom
            new StaticSprite(itemTexture, new Rectangle(32, 8, 16, 16)),   // fire flower
            new StaticSprite(itemTexture, new Rectangle(142, 8, 16, 16)),  // star
            new StaticSprite(itemTexture, new Rectangle(180, 36, 8, 15)),  // coin
        };
    }

    public List<ISprite> CreateBlockSprites()
    {
        return new List<ISprite>
        {
            new StaticSprite(blockTexture, new Rectangle(328, 128, 16, 16)),   // brick
            new StaticSprite(blockTexture, new Rectangle(180, 7, 16, 16)),  // question block
            new StaticSprite(blockTexture, new Rectangle(180, 116, 16, 16)),  // blue brick
            new StaticSprite(blockTexture, new Rectangle(180, 332, 16, 16)),  // grey block
        };
    }
}