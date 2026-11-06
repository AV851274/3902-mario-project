using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Mario.Game.Assets;

public sealed class SpriteDefinition
{
    public required string Kind { get; init; }
    public required string Texture { get; init; }
    public required float Scale { get; init; }

    // Static sprites use Rect; animated sprites use InitialClip and Clips.
    public Rectangle Rect { get; init; }
    public string InitialClip { get; init; } = "";
    public Dictionary<string, AnimationClipDefinition> Clips { get; init; } = [];
}
