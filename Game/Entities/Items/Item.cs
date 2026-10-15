using Mario.Interfaces;
using Mario.Physics;
using Mario.Physics.Aspects;
using Mario.Physics.State;
using Mario.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Players;
using Mario.Interfaces.Player;

namespace Mario.Items;

public class Item : IItem
{
    private const float MushroomSpeed = 100f;
    private const float Gravity = 1000f;
    private const float GroundY = 384f;
    private const int Size = 48;
    private readonly PhysicsController physicsController;
    private readonly HorizontalVelocityPhysics horizontalMovement;
    private readonly GravityPhysics gravityPhysics;
    public bool IsCollected { get; private set; }
    public Rectangle Bounds => new Rectangle(
        (int)physicsState.Position.X,
        (int)physicsState.Position.Y,
        Size,
        Size);

    private ISprite sprite;
    private ItemType type;
    private PhysicsState physicsState;
    private float direction = 1f;

    public Vector2 Position => physicsState.Position;

    public Item(SpriteFactory spriteFactory, ItemType type, Vector2 position, float bounceHeight = 80f)
    {
        this.type = type;
        physicsState = new PhysicsState(position, Vector2.Zero);
        sprite = spriteFactory.CreateItemSprite(type);
        horizontalMovement = new HorizontalVelocityPhysics(0f);
        gravityPhysics = new GravityPhysics(Gravity)
        {
            Enabled = type == ItemType.Mushroom || type == ItemType.Star
        };

        if (type == ItemType.Star)
        {
            physicsController = new PhysicsController
            {
                HorizontalVelocityPhysics = horizontalMovement,
                GravityPhysics = gravityPhysics,
                MovementPhysics = new MovementPhysics(),
                BouncingPhysics = new BouncingPhysics(
                    bounceHeight,
                    groundY: GroundY,
                    gravity: Gravity)
            };
        }
        else if (type == ItemType.Mushroom)
        {
            physicsController = new PhysicsController
            {
                HorizontalVelocityPhysics = horizontalMovement,
                GravityPhysics = gravityPhysics,
                MovementPhysics = new MovementPhysics(),
                GroundCollisionPhysics = new GroundCollisionPhysics(GroundY)
            };
        }
        else
        {
            physicsController = new PhysicsController
            {
                HorizontalVelocityPhysics = horizontalMovement,
                MovementPhysics = new MovementPhysics()
            };
        }
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
        horizontalMovement.VelocityX =
            type == ItemType.Mushroom || type == ItemType.Star
                ? direction * MushroomSpeed
                : 0f;

        physicsController.Update(ref physicsState, (float)gameTime.ElapsedGameTime.TotalSeconds);

        sprite.UpdateAnimation(gameTime);
    }
    public void BounceOffWall(float newX)
    {
        physicsState.Position = new Vector2(newX, physicsState.Position.Y);
        direction = -direction;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, physicsState.Position);
    }
}
