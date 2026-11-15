using Mario.Game.Interfaces;

namespace Mario.Game.Commands;

public class ToggleMusicCommand : ICommand
{
    private readonly IGameAudio audio;

    public ToggleMusicCommand(IGameAudio audio)
    {
        this.audio = audio;
    }

    public void Execute()
    {
        audio.ToggleMusicMute();
    }
}
