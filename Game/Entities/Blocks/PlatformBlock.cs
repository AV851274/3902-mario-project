using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;

namespace Mario.Game.Entities.Blocks;

public sealed class PlatformBlock : BlockBase
{
    public PlatformBlock(ISprite sprite, BlockType type, Vector2 position) : base(sprite, type, position) { }

    public override ICollider.BlockingModes BlockingMode => ICollider.BlockingModes.TopOnly;
}
