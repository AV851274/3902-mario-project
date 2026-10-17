using Mario.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Enemies;

public class Goomba : IEnemy
{
    private const float WalkSpeed = 60f;
    private const float Gravity = 1000f;
    private const int Size = 32;

    private ISprite sprite;
    private Vector2 position;
    private float velocityX = WalkSpeed;
    private float velocityY;
    private bool isStomped;

    public Vector2 Position => position;
    public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, Size, Size);
    public bool IsDefeated => isStomped;
    public bool CanHurtPlayer => !isStomped;

    public Goomba(ISprite sprite, Vector2 initialPosition)
    {
        this.sprite = sprite;
        position = initialPosition;
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        velocityY += Gravity * deltaTime;
        position.Y += velocityY * deltaTime;

        if (!isStomped)
        {
            position.X += velocityX * deltaTime;
        }

        sprite.UpdateAnimation(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = isStomped ? SpriteEffects.FlipVertically : SpriteEffects.None;
        sprite.Draw(spriteBatch, position, effects: effects);
    }

    public void Stomp()
    {
        if (isStomped)
            return;

        sprite.Play("Stomped");
        isStomped = true;
    }

    public void LandOn(float groundTop)
    {
        // A stomped Goomba falls through the floor and off the screen
        if (isStomped)
        {
            return;
        }

        position.Y = groundTop - Size;
        velocityY = 0f;
    }

    public void BounceOffWall(float newX)
    {
        position.X = newX;
        velocityX = -velocityX;
    }
}
