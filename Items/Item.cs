using Mario.Interfaces;
using Mario.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Mario.Items;

public class Item : IItem
{
    //A good portion of this is temporary, as a lot of functionality will be changed in later sprints.

    private const float MushroomSpeed = 100f;
    private const float StarBounceSpeed = 400f;
    private const float Gravity = 1000f;
    private const float GroundY = 400f;
    private List<ISprite> sprites;
    private int currentSpriteIndex = 0;
    private Vector2 initialPosition;
    private Vector2 position;
    private Vector2 velocity;
    private static readonly string[] itemNames = ["Mushroom", "Fire Flower", "Coin", "Star"];

    public Vector2 Position => position;
    public string CurrentItemName => itemNames[currentSpriteIndex];

    public Item(List<ISprite> sprites, Vector2 position)
    {
        this.sprites = sprites;
        this.position = position;
        initialPosition = position;
    }

    public void nextSprite()
    {
        currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Count;
        position = initialPosition;
        velocity = Vector2.Zero;
    }

    public void prevSprite()
    {
        currentSpriteIndex = (currentSpriteIndex - 1 + sprites.Count) % sprites.Count;
        position = initialPosition;
        velocity = Vector2.Zero;
    }

    public void Update(GameTime gameTime)
    {
        if (CurrentItemName == "Star")
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
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
        else if (CurrentItemName == "Mushroom")
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocity.X = MushroomSpeed;
            GravityPhysics.Apply(ref position, ref velocity, deltaTime, Gravity, GroundY);
        }

        sprites[currentSpriteIndex].UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprites[currentSpriteIndex].Draw(spriteBatch, Position);
    }
}
