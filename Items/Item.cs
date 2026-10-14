using Mario.Interfaces;
using Mario.Physics;
using Mario.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Players;
using Mario.Interfaces.Player;

namespace Mario.Items;

public class Item : IItem
{
    private const float MushroomSpeed = 100f;
    private const float StarBounceSpeed = 400f;
    private const float Gravity = 1000f;
    private const float GroundY = 384f;
    private const int Size = 48;
    public bool IsCollected { get; private set; }
    public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, Size, Size);

    private ISprite sprite;
    private ItemType type;
    private Vector2 position;
    private Vector2 velocity;
    private float direction = 1f;

    public Vector2 Position => position;

    public Item(SpriteFactory spriteFactory, ItemType type, Vector2 position)
    {
        this.type = type;
        this.position = position;
        sprite = spriteFactory.CreateItemSprite(type);
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
            // Like the original: Small Mario only grows to Big from a flower
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
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (type == ItemType.Star)
        {
            velocity.X = direction * MushroomSpeed;

            BouncingPhysics.Apply(
                ref position,
                ref velocity,
                deltaTime,
                Gravity,
                GroundY,
                StarBounceSpeed);
        }
        else if (type == ItemType.Mushroom)
        {
            velocity.X = direction * MushroomSpeed;
            GravityPhysics.Apply(ref position, ref velocity, deltaTime, Gravity);
            if (position.Y >= GroundY)
            {   
                position.Y = GroundY;
                velocity.Y = 0f;
            }
        }

        sprite.UpdateAnimation(gameTime);
    }
    public void BounceOffWall(float newX)
    {
        position.X = newX;
        direction = -direction;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, position);
    }
}
