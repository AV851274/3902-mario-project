using System.Linq;
using System;
using Mario.Game.Animation;
using Mario.Game.Assets;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;

namespace Mario.Game.Rendering.Sprites;


// Builds a fresh sprite from a JSON definition using AssetManager's shared texture.
// Static definitions provide a rectangle; animated definitions provide named clips.
public sealed class SpriteFactory : ISpriteFactory
{
    private readonly AssetManager assets;

    public SpriteFactory(AssetManager assets)
    {
        this.assets = assets;
    }

    public ISprite Create(SpriteDefinition definition)
    {
        var texture = assets.GetTexture(definition.Texture);
        if (definition.Kind == "static")
            return new StaticSprite(texture, definition.Rect, definition.Scale);

        // Each entity gets its own playback state while sharing the loaded texture.
        var sprite = new AnimationController(texture, Vector2.Zero, definition.Scale);
        foreach (var (name, clip) in definition.Clips)
        {
            sprite.AddClip(new Track(name,
                clip.Frames.Select(frame => frame.Rect).ToArray(),
                clip.Frames.Select(frame => frame.Duration).ToArray(), clip.Loop));
        }

        if (!string.IsNullOrEmpty(definition.InitialClip))
            sprite.Play(definition.InitialClip);
        return sprite;
    }

    // String IDs match JSON keys exactly, e.g. Create("enemies", "goomba").
    public ISprite Create(string category, string id) =>
        Create(assets.GetSpriteDefinition(category, id));

    // Enum names use camelCase JSON keys, e.g. UsedQuestion -> "usedQuestion".
    public ISprite Create(string category, Enum type) =>
        Create(category, AssetManager.Naming.ConvertName(type.ToString()));
}
