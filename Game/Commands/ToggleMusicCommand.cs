using Mario.Game.Audio;
using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public class ToggleMusicCommand : ICommand
{
    private readonly GameAudio audio;

    public ToggleMusicCommand(GameAudio audio)
    {
        this.audio = audio;
    }

    public void Execute()
    {
        audio.ToggleMusicMute();
    }
}