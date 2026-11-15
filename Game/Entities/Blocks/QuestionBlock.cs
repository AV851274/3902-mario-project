using System;
using Mario.Game.Audio;
using Mario.Game.Entities.Items;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;

namespace Mario.Game.Entities.Blocks;

public sealed class QuestionBlock : BlockBase
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

    private readonly ISprite usedSprite;
    private readonly ItemFactory itemFactory;
    private readonly IEntitySpawner spawner;
    private readonly IGameAudio audio;
    private ItemType? contents;
    private bool isUsed;

    public QuestionBlock(ISprite sprite, ISprite usedSprite, Vector2 position,
        ItemType? contents, ItemFactory itemFactory, IEntitySpawner spawner, IGameAudio audio)
        : base(sprite, BlockType.Question, position)
    {
        this.usedSprite = usedSprite;
        this.contents = contents;
        this.itemFactory = itemFactory;
        this.spawner = spawner;
        this.audio = audio;
    }

    public override void Update(GameTime gameTime)
    {
        bumpTimer = Math.Max(0f, bumpTimer - (float)gameTime.ElapsedGameTime.TotalSeconds);
        base.Update(gameTime);
    }

    public override void Hit(bool big)
    {
        if (isUsed)
            return;

        bumpTimer = BumpDuration;
        if (contents.HasValue)
        {
            ItemType released = contents.Value;
            if (released == ItemType.Mushroom && big)
                released = ItemType.FireFlower;

            var item = itemFactory.Create(released, Position);
            item.StartEmerging();
            spawner.Spawn(item);
            if (released != ItemType.Coin)
                audio.PlaySound(SoundType.Item);

            contents = null;
        }

        isUsed = true;
        Sprite = usedSprite;
    }
}
