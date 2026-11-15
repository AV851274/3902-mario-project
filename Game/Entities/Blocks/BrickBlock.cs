using System;
using Mario.Game.Audio;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Blocks;

public sealed class BrickBlock : BlockBase
{
    private const float BumpHeight = 12f;
    private const float BumpDuration = 0.15f;
    private float bumpTimer;

    protected override Vector2 DrawPosition
    {
        get
        {
            float offset = bumpTimer > 0f
                ? MathF.Sin((1f - bumpTimer / BumpDuration) * MathF.PI) * BumpHeight
                : 0f;
            return Position - new Vector2(0f, offset);
        }
    }

    private readonly IGameAudio audio;
    private bool isBroken;

    public BrickBlock(ISprite sprite, Vector2 position, IGameAudio audio) : base(sprite, BlockType.Brick, position)
    {
        this.audio = audio;
    }

    public override ICollider.BlockingModes BlockingMode =>
        isBroken ? ICollider.BlockingModes.None : ICollider.BlockingModes.Solid;

    public override void Update(GameTime gameTime)
    {
        bumpTimer = Math.Max(0f, bumpTimer - (float)gameTime.ElapsedGameTime.TotalSeconds);
        base.Update(gameTime);
    }

    public override void Hit(bool big)
    {
        bumpTimer = BumpDuration;
        isBroken = big;
        audio.PlaySound(isBroken ? SoundType.Brick : SoundType.Bump);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!isBroken)
            base.Draw(spriteBatch);
    }
}
