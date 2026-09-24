using System;
using Microsoft.Xna.Framework;

namespace Game2D.Animation
{
    /// <summary>
    /// A named sequence of spritesheet frames, each with its own display duration.
    /// Frames are given as explicit source Rectangles rather than computed from a fixed
    /// row/column grid, so a hand-packed or irregular spritesheet works just as well as
    /// a uniform one - which matters since these coordinates are set up by hand.
    /// </summary>
    public class Track
    {
        private string name;
        private Rectangle[] frames;
        private float[] frameDurationsSeconds;
        private bool isLooping;

        public Track(string name, Rectangle[] frames, float[] frameDurationsSeconds, bool isLooping)
        {
            if (frames.Length == 0)
            {
                throw new ArgumentException("An animation track needs at least one frame.");
            }
            if (frames.Length != frameDurationsSeconds.Length)
            {
                throw new ArgumentException("Frame count and frame-duration count must match.");
            }

            this.name = name;
            this.frames = frames;
            this.frameDurationsSeconds = frameDurationsSeconds;
            this.isLooping = isLooping;
        }

        public string Name
        {
            get { return name; }
        }

        public bool IsLooping
        {
            get { return isLooping; }
        }

        public int FrameCount
        {
            get { return frames.Length; }
        }

        public Rectangle GetFrame(int index)
        {
            return frames[index];
        }

        public float GetFrameDuration(int index)
        {
            return frameDurationsSeconds[index];
        }
    }
}
