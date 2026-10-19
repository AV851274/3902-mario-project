using Mario.Game.Physics;
using Mario.Game.Interfaces;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Game.Entities.Players;
using Mario.Game.Interfaces.Player;

namespace Mario.Game.Entities.Items;

public class Item : IItem, IPhysicsBody
{
    private const float MushroomSpeed = 100f;
    private const int Size = 48;
    public bool IsCollected { get; private set; }
    public bool Active => !IsCollected;
    public Box Bounds => new Box(
        Position.X,
        Position.Y,
        Size,
        Size);

    private ISprite sprite;
    private ItemType type;
    private float direction = 1f;

    public Vector2 PendingAcceleration { get; set; }
    public bool IsOnGround { get; set; }
    public float GravityScale => type == ItemType.Mushroom || type == ItemType.Star ? 1f : 0f;
    public float BounceSpeed { get; }
    public bool CollisionEnabled => Active;
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }

    public Item(SpriteFactory spriteFactory, ItemType type, Vector2 position, float bounceSpeed = 400f)
    {
        this.type = type;
        Position = position;
        sprite = spriteFactory.CreateItemSprite(type);
        if (!float.IsFinite(bounceSpeed) || bounceSpeed < 0f)
            throw new System.ArgumentOutOfRangeException(nameof(bounceSpeed));
        BounceSpeed = type == ItemType.Star ? bounceSpeed : 0f;
    }

    public void Collect(IPlayer player)
    {
        if (IsCollected)
        {
            return;
        }

        IsCollected = true;
        MarioPower power = player.MarioPowerState.Power;

        if (type == ItemType.Mushroom)
        {
            if (power == MarioPower.Small)
            {
                player.SetPower(MarioPower.Big);
            }
        }
        else if (type == ItemType.FireFlower)
        {
            // Like the original: Small Mario.Game.Bootstrap only grows to Big from a flower
            if (power == MarioPower.Small)
            {
                player.SetPower(MarioPower.Big);
            }
            else
            {
                player.SetPower(MarioPower.Fire);
            }
        }
        //else if (type == ItemType.Coin)
        //{
            // TODO: add to a coin counter once there's a HUD
        //}
        //else if (type == ItemType.Star)
        //{
            // TODO: star invincibility
        //}
    }

    public void Update(GameTime gameTime)
    {
        float speed = type == ItemType.Mushroom || type == ItemType.Star
            ? direction * MushroomSpeed
            : 0f;
        Velocity = new Vector2(speed, Velocity.Y);

        sprite.UpdateAnimation(gameTime);
    }
    public void OnCollision(in CollisionEvent collision)
    {
        if (collision.Other is IBlock && collision.Normal.X != 0f)
        {
            direction = -direction;
            Velocity = new Vector2(-collision.RelativeVelocityBefore.X, Velocity.Y);
        }
        else if (collision.Other is IBlock && collision.Normal.Y < 0f && IsOnGround && BounceSpeed > 0f)
        {
            Velocity = new Vector2(Velocity.X, -BounceSpeed);
            IsOnGround = false;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, Position);
    }
}
