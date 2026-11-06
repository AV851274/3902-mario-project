using Microsoft.Xna.Framework;
namespace Mario.Game.Assets;

public sealed class AnimationFrameDefinition
{
    /// <summary>Source rectangle in texture pixels.</summary>
    public required Rectangle Rect { get; init; }

    /// <summary>Frame duration in seconds.</summary>
    public required float Duration { get; init; }
}
