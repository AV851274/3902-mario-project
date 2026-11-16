using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;

namespace Mario.Game.Entities.Blocks;

public sealed class WarpPipeBlock : BlockBase
{
    public WarpPipeBlock(ISprite sprite, BlockType type, Vector2 position) : base(sprite, type, position) { }
}
