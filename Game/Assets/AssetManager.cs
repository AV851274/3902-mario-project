using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Mario.Game.Assets;

// Loads the JSON manifest and sprite definitions once; callers use logical IDs
// instead of content paths to retrieve shared textures, audio, and fonts.
public sealed class AssetManager
{

    public static readonly JsonNamingPolicy Naming = JsonNamingPolicy.CamelCase;
    private static readonly JsonSerializerOptions JsonOptions =
        new() { PropertyNamingPolicy = Naming, IncludeFields = true };

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

        // Sprite definition file paths are relative to the manifest's directory.
        string directory = Path.GetDirectoryName(path)!;
        foreach (var (category, file) in manifest.SpriteFiles)
        {
            var definitions = ReadJson<SpriteDefinitionFile>(Path.Combine(directory, file));
            sprites.Add(category, definitions.Sprites);
        }
    }

    // Definitions are grouped by category, e.g. ("mario", "small.swimming").
    public SpriteDefinition GetSpriteDefinition(string category, string id) => sprites[category][id];

    // Getters return loaded resources; they do not reload or create copies.
    public Texture2D GetTexture(string id) => textures[id];
    public SoundEffect GetSound(string id) => sounds[id];
    public Song GetMusic(string id) => music[id];
    public SpriteFont GetFont(string id) => fonts[id];

    private static Dictionary<string, T> LoadAssets<T>(ContentManager content, Dictionary<string, string> names)
    {
        var assets = new Dictionary<string, T>();
        // Manifest values are ContentManager asset paths, relative to its root.
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
