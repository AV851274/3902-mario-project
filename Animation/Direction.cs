namespace Game2D.Animation
{
    /// <summary>
    /// The spritesheet only draws the character facing right; Left is achieved by
    /// horizontally flipping that same art rather than storing a mirrored copy.
    /// </summary>
    public enum Direction
    {
        Right,
        Left
    }
}
