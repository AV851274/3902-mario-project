using Mario.Game.Assets;
using Mario.Game.Interfaces;

namespace Mario.Game.Audio;

public sealed class GameAudio : IGameAudio
{
    private readonly SoundManager sounds;

    public GameAudio(AssetManager assets)
    {
        sounds = new SoundManager(assets);
    }

    public void StartRoomMusic()
    {
        sounds.PlayMusic();
    }

    public void PlaySound(SoundType sound)
    {
        sounds.Play(sound);
    }

    public void PlayDeathAudio()
    {
        sounds.StopMusic();
        sounds.Play(SoundType.Death);
    }

    public void PlayFlagpoleAudio()
    {
        sounds.StopMusic();
    }

    public void ToggleMusicMute()
    {
        sounds.ToggleMusicMute();
    }
}
