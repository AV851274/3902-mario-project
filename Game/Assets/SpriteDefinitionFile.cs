using System.Collections.Generic;

namespace Mario.Game.Assets;

public sealed class SpriteDefinitionFile
{
    public required Dictionary<string, SpriteDefinition> Sprites { get; init; }
}
