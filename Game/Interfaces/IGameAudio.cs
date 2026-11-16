using Mario.Game.Audio;

namespace Mario.Game.Interfaces;

public interface IGameAudio
{
    void PlaySound(SoundType sound);
    void PlayDeathAudio();
    void PlayFlagpoleAudio();
    void StartRoomMusic();
    void ToggleMusicMute();
}
