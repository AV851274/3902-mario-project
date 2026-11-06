using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Mario.Game.Assets;

public sealed class AssetManager
{
    private static readonly JsonSerializerOptions JsonOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, IncludeFields = true };

    private readonly Dictionary<string, Texture2D> textures;
    private readonly Dictionary<string, SoundEffect> sounds;
    private readonly Dictionary<string, Song> music;
    private readonly Dictionary<string, SpriteFont> fonts;
    private readonly Dictionary<string, Dictionary<string, SpriteDefinition>> sprites = [];

    // The game owns ContentManager and releases its cached resources.
    public AssetManager(ContentManager content, string manifestFile)
    {
        string path = Path.Combine(content.RootDirectory, manifestFile);
        var manifest = ReadJson<AssetManifest>(path);

        textures = LoadAssets<Texture2D>(content, manifest.Textures);
        sounds = LoadAssets<SoundEffect>(content, manifest.Sounds);
        music = LoadAssets<Song>(content, manifest.Music);
        fonts = LoadAssets<SpriteFont>(content, manifest.Fonts);

        string directory = Path.GetDirectoryName(path)!;
        foreach (var (category, file) in manifest.SpriteFiles)
        {
            var definitions = ReadJson<SpriteDefinitionFile>(Path.Combine(directory, file));
            sprites.Add(category, definitions.Sprites);
        }
    }

    public SpriteDefinition GetSpriteDefinition(string category, string id) => sprites[category][id];

    public Texture2D GetTexture(string id) => textures[id];
    public SoundEffect GetSound(string id) => sounds[id];
    public Song GetMusic(string id) => music[id];
    public SpriteFont GetFont(string id) => fonts[id];

    private static Dictionary<string, T> LoadAssets<T>(ContentManager content, Dictionary<string, string> names)
    {
        var assets = new Dictionary<string, T>();
        foreach (var (id, name) in names)
            assets.Add(id, content.Load<T>(name));
        return assets;
    }

    private static T ReadJson<T>(string path)
    {
        using var stream = Path.IsPathRooted(path)
            ? File.OpenRead(path)
            : TitleContainer.OpenStream(path);
        return JsonSerializer.Deserialize<T>(stream, JsonOptions)!;
    }
}