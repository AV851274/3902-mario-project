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
        effects[SoundType.SmallJump] = content.Load<SoundEffect>("smallJump");
        effects[SoundType.BigJump] = content.Load<SoundEffect>("bigJump");
        effects[SoundType.Brick] = content.Load<SoundEffect>("brick");
        effects[SoundType.Bump] = content.Load<SoundEffect>("bump");
        effects[SoundType.Coin] = content.Load<SoundEffect>("coin");
        effects[SoundType.Death] = content.Load<SoundEffect>("death");
        effects[SoundType.Fireball] = content.Load<SoundEffect>("fireball");
        effects[SoundType.Item] = content.Load<SoundEffect>("item");
        effects[SoundType.PowerUp] = content.Load<SoundEffect>("powerup");
        effects[SoundType.StompSwim] = content.Load<SoundEffect>("stompswim");

        music = content.Load<Song>("overworld");
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
