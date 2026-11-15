using Mario.Game.Interfaces;
using Mario.Game.Physics;
using Microsoft.Xna.Framework;

namespace Mario.Game.Entities.Blocks;

public sealed class FlagPoleBlock : BlockBase
{
    private const int Height = 480;

    public FlagPoleBlock(ISprite sprite, Vector2 position) : base(sprite, BlockType.FlagPole, position) { }

    public override Box Bounds => new(Position.X, Position.Y, Size, Height);
    public override ICollider.BlockingModes BlockingMode => ICollider.BlockingModes.ThroughWalls;
}
