using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;

namespace Mario.Game.Entities.Blocks;

public sealed class SolidBlock : BlockBase
{
    public SolidBlock(ISprite sprite, BlockType type, Vector2 position) : base(sprite, type, position) { }
}
