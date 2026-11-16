using Mario.Game.Interfaces;
using Mario.Game.Physics;
using Microsoft.Xna.Framework;

namespace Mario.Game.Entities.Blocks;

public sealed class FlagPoleBlock : BlockBase
{
    private const int HeightInTiles = 10;
    private const int Height = GameConstants.World.TileSize * HeightInTiles;

    public FlagPoleBlock(ISprite sprite, Vector2 position) : base(sprite, BlockType.FlagPole, position) { }

    public override Box Bounds => new(Position.X, Position.Y, GameConstants.World.TileSize, Height);
    public override ICollider.BlockingModes BlockingMode => ICollider.BlockingModes.ThroughWalls;
}
