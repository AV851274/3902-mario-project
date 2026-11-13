using Mario.Game.Assets;
using Mario.Game.Entities.Items;
using Mario.Game.Interfaces;
using Mario.Game.Interfaces.Player;

namespace Mario.Game.Audio;

public sealed class GameAudio
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

    public void Register(IBlock block)
    {
        block.SoundRequested += sounds.Play;
    }

    public void Register(IItem item)
    {
        item.Collected += OnItemCollected;
    }

    public void Register(IPlayer player)
    {
        player.SoundRequested += sounds.Play;
        player.DeathStarted += OnMarioDeathStarted;
        player.FlagpoleReached += OnFlagpoleReached;
    }

    private void OnItemCollected(ItemType type)
    {
        if (type == ItemType.Coin)
            sounds.Play(SoundType.Coin);
        else
            sounds.Play(SoundType.PowerUp);
    }

    private void OnMarioDeathStarted()
    {
        sounds.StopMusic();
        sounds.Play(SoundType.Death);
    }

    private void OnFlagpoleReached()
    {
        sounds.StopMusic();
    }

    public void ToggleMusicMute()
    {
        sounds.ToggleMusicMute();
    }
}