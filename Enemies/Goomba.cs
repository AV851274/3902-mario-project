using Mario.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Enemies;

public class Goomba : IEnemy
{
    private const float WalkSpeed = 60f;

    private ISprite sprite;
    private Vector2 position;

    private float rotation = 0f;

    private const float Gravity = 1000f;

    private float velocityY;
    private float velocityX = -WalkSpeed;
    private float leftBound;
    private float rightBound;
    private bool isStomped;

    public Vector2 Position
    {
        get { return position; }
    }

    public Goomba(ISprite sprite, Vector2 initialPosition, int patrolDistance = 150)
    {
        this.sprite = sprite;
        position = initialPosition;
        leftBound = initialPosition.X - patrolDistance;
        rightBound = initialPosition.X + patrolDistance;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, position, rotation);
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (!isStomped)
        {
            position.X += velocityX * deltaTime;

            //TURN GOOMBA AROUND WHEN IT HITS THE EDGE OF ITS PATROL AREA
            if (position.X <= leftBound || position.X >= rightBound)
            {
                velocityX = -velocityX;
            }
        } else
        {
            position.Y += velocityY * deltaTime;
            velocityY += Gravity * deltaTime;
        }

        sprite.UpdateAnimation(gameTime);
    }

    public void Stomp()
    {
        if (isStomped)
            return;
        
        sprite.Play("Stomped");
        rotation = MathHelper.ToRadians(180f);
        isStomped = true;
    }
}
