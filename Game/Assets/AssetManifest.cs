using System.Collections.Generic;

namespace Mario.Game.Assets;

public sealed class AssetManifest
{
    public required Dictionary<string, string> Textures { get; init; }

    public required Dictionary<string, string> Sounds { get; init; }

    public required Dictionary<string, string> Music { get; init; }

    public required Dictionary<string, string> Fonts { get; init; }

    public required Dictionary<string, string> SpriteFiles { get; init; }
}
