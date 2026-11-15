using Mario.Game.Interfaces;
using Mario.Game.Interfaces.Player;
using Mario.Game.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Blocks;

public sealed class CloudBlock : BlockBase
{
    private const float DisappearDelay = 2f;
    private const float ReappearTime = -2f;
    private bool timerStarted;
    private float timer;
    private bool IsHidden => timerStarted && timer <= 0f;

    public CloudBlock(ISprite sprite, Vector2 position) : base(sprite, BlockType.Cloud, position) { }

    public override ICollider.BlockingModes BlockingMode =>
        IsHidden ? ICollider.BlockingModes.None : ICollider.BlockingModes.TopOnly;

    public override void OnCollision(CollisionEvent collision)
    {
        if (collision.Normal.Y > 0f &&
            (collision.Other is IPlayer || collision.Other is IEnemy) &&
            !timerStarted && collision.RelativeVelocityBefore.Y < 0f)
        {
            timerStarted = true;
            timer = DisappearDelay;
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (timerStarted)
        {
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer <= ReappearTime)
            {
                timerStarted = false;
                timer = 0f;
            }
        }
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!IsHidden)
            base.Draw(spriteBatch);
    }
}
