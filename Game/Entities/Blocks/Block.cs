using Mario.Game.Physics;
using System;
using Mario.Game.Audio;
using Mario.Game.Interfaces;
using Mario.Game.Entities.Items;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Game.Interfaces.Player;

namespace Mario.Game.Entities.Blocks;

public class Block : IBlock
{
    private const int Size = 48;
    private const int FlagPoleHeight = 480;
    private const float BumpHeight = 12f;
    private const float BumpDuration = 0.15f;

    private readonly IEntitySpawner spawner;
    private SpriteFactory spriteFactory;
    private ISprite sprite;
    private BlockType type;
    private ItemType? contents;
    private float bumpTimer;
    private float cloudDisappearTime = 2f;
    private float cloudComebackTime = -2f;
    private bool cloudTimerStarted = false;
    private float cloudTimer = 0;
    public bool Active => true;
    private bool isIntact = true;
    public bool IsWarpPipe => type == BlockType.WarpPipeTopLeft || type == BlockType.WarpPipeTopRight;
    public bool IsFlagPole => type == BlockType.FlagPole;

    public Vector2 Position { get; private set; }

    public Box Bounds => IsFlagPole
        ? new Box(Position.X, Position.Y, Size, FlagPoleHeight)
        : new Box(Position.X, Position.Y, Size, Size);


    public event Action<SoundType> SoundRequested;

    public Block(SpriteFactory spriteFactory, IEntitySpawner spawner, BlockType type, Vector2 position, ItemType? contents = null)
    {
        this.spawner = spawner;
        this.spriteFactory = spriteFactory;
        this.type = type;
        this.contents = contents;
        sprite = spriteFactory.Create("blocks", type);
        Position = position;
    }

    public ICollider.BlockingModes BlockingMode =>
        IsFlagPole ? ICollider.BlockingModes.ThroughWalls : GetBlockingMode();

    private ICollider.BlockingModes GetBlockingMode()
    {
        if (type == BlockType.Cloud)
        {
            return cloudTimerStarted && cloudTimer <= 0f
                ? ICollider.BlockingModes.None
                : ICollider.BlockingModes.TopOnly;
        }

        if (type == BlockType.MovingPlatform || type == BlockType.Bridge)
            return ICollider.BlockingModes.TopOnly;

        if (!isIntact)
            return ICollider.BlockingModes.None;

        return ICollider.BlockingModes.Solid;
    }

    public void OnCollision(CollisionEvent collision)
    {
        //Set timer to despawn cloud if it's been landed on.
        if (type == BlockType.Cloud &&
            collision.Normal.Y > 0f &&
            (collision.Other is IPlayer || collision.Other is IEnemy) &&
            !cloudTimerStarted &&
            collision.RelativeVelocityBefore.Y < 0f)
        {
            cloudTimerStarted = true;
            cloudTimer = cloudDisappearTime;
        }

        //bool stomped = collision.Normal.Y < 0f && collision.RelativeVelocityBefore.Y > 0f;
    }

    public void Hit(bool big)
    {
        if (type != BlockType.Brick && type != BlockType.Question)
        {
            return;
        }

        bumpTimer = BumpDuration;

        if (type == BlockType.Question)
        {
            ReleaseContents(big);
            type = BlockType.UsedQuestion;
            sprite = spriteFactory.Create("blocks", BlockType.UsedQuestion);
        }

        isIntact = !(type == BlockType.Brick && big);

        if (!isIntact)
        {
            SoundRequested?.Invoke(SoundType.Brick);
        }
        else if (type == BlockType.Brick)
        {
            SoundRequested?.Invoke(SoundType.Bump);
        }
    }

    private void ReleaseContents(bool big)
    {
        if (!contents.HasValue)
        {
            return;
        }

        ItemType released = contents.Value;

        if (released == ItemType.Mushroom && big)
        {
            released = ItemType.FireFlower;
        }

        var item = new Item(spriteFactory, released, Position);
        item.StartEmerging();
        spawner.Spawn(item);
        if (released != ItemType.Coin)
            SoundRequested?.Invoke(SoundType.Item);

        contents = null;
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        bumpTimer = Math.Max(0f, bumpTimer - deltaTime);

        if (cloudTimerStarted)
            cloudTimer -= deltaTime;

        if (cloudTimerStarted && cloudTimer <= cloudComebackTime)
        {
            cloudTimerStarted = false;
            cloudTimer = 0;
        }

        sprite.UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (cloudTimerStarted && cloudTimer <= 0f)
            return;
        if (!isIntact)
            return;
        sprite.Draw(spriteBatch, Position - new Vector2(0, BumpOffset()));
    }

    private float BumpOffset()
    {
        if (bumpTimer <= 0f)
        {
            return 0f;
        }

        float progress = 1f - (bumpTimer / BumpDuration);
        return MathF.Sin(progress * MathF.PI) * BumpHeight;
    }
}