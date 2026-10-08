using Mario.Interfaces;
using Mario.Physics;
using Mario.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Items;

public class Item : IItem
{
    private const float MushroomSpeed = 100f;
    private const float StarBounceSpeed = 400f;
    private const float Gravity = 1000f;
    private const float GroundY = 400f;

    private ISprite sprite;
    private ItemType type;
    private Vector2 position;
    private Vector2 velocity;

    public Vector2 Position => position;

    public Item(SpriteFactory spriteFactory, ItemType type, Vector2 position)
    {
        this.type = type;
        this.position = position;
        sprite = spriteFactory.CreateItemSprite(type);
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (type == ItemType.Star)
        {
            if (velocity.X == 0f)
            {
                velocity.X = MushroomSpeed;
            }

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
            velocity.X = MushroomSpeed;
            GravityPhysics.Apply(ref position, ref velocity, deltaTime, Gravity, GroundY);
        }

        sprite.UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, position);
    }
}
