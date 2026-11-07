using System.Linq;
using System.Text.Json;
using Mario.Game.Animation;
using Mario.Game.Assets;
using Mario.Game.Entities.Background;
using Mario.Game.Entities.Blocks;
using Mario.Game.Entities.Items;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;

namespace Mario.Game.Rendering.Sprites;

public sealed class SpriteFactory
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

    public ISprite Create(BlockType type) =>
        Create(assets.GetSpriteDefinition("blocks", JsonNamingPolicy.CamelCase.ConvertName(type.ToString())));

    public ISprite Create(ItemType type) =>
        Create(assets.GetSpriteDefinition("items", JsonNamingPolicy.CamelCase.ConvertName(type.ToString())));

    public ISprite Create(BackgroundType type) =>
        Create(assets.GetSpriteDefinition("background", JsonNamingPolicy.CamelCase.ConvertName(type.ToString())));

    public ISprite CreateMario(string id) => Create(assets.GetSpriteDefinition("mario", id));
    public ISprite CreateEnemy(string id) => Create(assets.GetSpriteDefinition("enemies", id));
    public ISprite CreateProjectile(string id) => Create(assets.GetSpriteDefinition("projectiles", id));
}