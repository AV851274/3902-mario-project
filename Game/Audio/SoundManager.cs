using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Mario.Game.Audio;

public class SoundManager
{
    private readonly Dictionary<SoundType, SoundEffect> effects = new Dictionary<SoundType, SoundEffect>();
    private Song music;

    public void LoadContent(ContentManager content)
    {
        effects[SoundType.SmallJump] = content.Load<SoundEffect>("audio/sfx/smallJump");
        effects[SoundType.BigJump] = content.Load<SoundEffect>("audio/sfx/bigJump");
        effects[SoundType.Brick] = content.Load<SoundEffect>("audio/sfx/brick");
        effects[SoundType.Bump] = content.Load<SoundEffect>("audio/sfx/bump");
        effects[SoundType.Coin] = content.Load<SoundEffect>("audio/sfx/coin");
        effects[SoundType.Death] = content.Load<SoundEffect>("audio/sfx/death");
        effects[SoundType.Fireball] = content.Load<SoundEffect>("audio/sfx/fireball");
        effects[SoundType.Item] = content.Load<SoundEffect>("audio/sfx/item");
        effects[SoundType.PowerUp] = content.Load<SoundEffect>("audio/sfx/powerup");
        effects[SoundType.StompSwim] = content.Load<SoundEffect>("audio/sfx/stompswim");

        music = content.Load<Song>("audio/music/overworld");
    }

    public void Play(SoundType sound)
    {
        effects[sound].Play();
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
