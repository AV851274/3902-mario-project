namespace Mario.Game.Assets;

public sealed class AnimationClipDefinition
{
    public required bool Loop { get; init; }

    public required AnimationFrameDefinition[] Frames { get; init; }
}
