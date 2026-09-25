using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game2D.Interfaces;

namespace Game2D.Animation
{
    public class AnimationController : ISprite
    {
        /// <summary>
        /// Plays back a set of named AnimationClips against a single spritesheet texture.
        /// Unlike a single-strip animator, this holds a whole clip library keyed by name
        /// (e.g. "WalkDown", "IdleUp", "Hover") so one entity can switch between many
        /// animations without swapping textures or classes.
        /// </summary>
        private Texture2D spriteSheet;
        private Dictionary<string, Track> clips;
        private Vector2 origin;
        private float scale;

        private Track currentClip;
        private int frameIndex;
        private float timeInFrameSeconds;

        public AnimationController(Texture2D spriteSheet, Vector2 origin, float scale)
        {
            this.spriteSheet = spriteSheet;
            this.origin = origin;
            this.scale = scale;
            clips = new Dictionary<string, Track>();
            currentClip = null;
            frameIndex = 0;
            timeInFrameSeconds = 0f;
        }

        public void AddClip(Track clip)
        {
            clips.Add(clip.Name, clip);
        }

        public void Play(string clipName)
        {
            if (currentClip != null && currentClip.Name == clipName)
            {
                return;
            }

            Track clip;
            bool found = clips.TryGetValue(clipName, out clip);
            if (!found)
            {
                return;
            }

            currentClip = clip;
            frameIndex = 0;
            timeInFrameSeconds = 0f;
        }

        public void UpdateAnimation(GameTime gameTime)
        {
            if (currentClip == null)
            {
                return;
            }

            timeInFrameSeconds = timeInFrameSeconds + (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (timeInFrameSeconds >= currentClip.GetFrameDuration(frameIndex))
            {
                timeInFrameSeconds = timeInFrameSeconds - currentClip.GetFrameDuration(frameIndex);
                frameIndex = frameIndex + 1;

                if (frameIndex >= currentClip.FrameCount)
                {
                    if (currentClip.IsLooping)
                    {
                        frameIndex = 0;
                    }
                    else
                    {
                        frameIndex = currentClip.FrameCount - 1;
                    }
                }
            }
        }

        public void Draw(
            SpriteBatch spriteBatch,
            Vector2 position,
            float rotation = 0f,
            SpriteEffects effects = SpriteEffects.None)
        {
            if (currentClip == null)
            {
                return;
            }

            Rectangle sourceRectangle = currentClip.GetFrame(frameIndex);

            spriteBatch.Draw(spriteSheet, position, sourceRectangle, Color.White, rotation, origin, scale, effects, 0f);
        }
    }
}
