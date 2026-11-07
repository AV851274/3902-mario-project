using System.Text.Json;
using Mario.Game.Assets;
using Microsoft.Xna.Framework.Media;

namespace Mario.Game.Audio;

public class SoundManager
{
    private readonly AssetManager assets;
    private readonly Song music;

    public SoundManager(AssetManager assets)
    {
        this.assets = assets;
        music = assets.GetMusic("overworld");
    }

    public void Play(SoundType sound)
    {
        string id = JsonNamingPolicy.CamelCase.ConvertName(sound.ToString());
        assets.GetSound(id).Play();
    }

    public void PlayMusic()
    {
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Play(music);
    }

    public void StopMusic()
    {
        MediaPlayer.Stop();
    }

    public void ToggleMusicMute()
    {
        MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
    }
}
