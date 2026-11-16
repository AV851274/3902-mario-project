using Mario.Game.Interfaces;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;

namespace Mario.Game.Entities.Items;

public sealed class ItemFactory
{
    private readonly SpriteFactory sprites;
    private readonly IRewardReceiver rewards;
    private readonly IGameAudio audio;

    public ItemFactory(SpriteFactory sprites, IRewardReceiver rewards, IGameAudio audio)
    {
        this.sprites = sprites;
        this.rewards = rewards;
        this.audio = audio;
    }

    public Item Create(ItemType type, Vector2 position,
        float bounceSpeed = GameConstants.Projectiles.DefaultBounceSpeed) =>
        new Item(sprites, rewards, audio, type, position, bounceSpeed);
}
