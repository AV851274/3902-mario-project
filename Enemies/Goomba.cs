using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game2D.Interfaces;

public class Goomba : IEnemy
{
    private const float WalkSpeed = 60f;

    private ISprite sprite;
    private Vector2 position;
    private float velocityX = -WalkSpeed;
    private float leftBound;
    private float rightBound;
    private bool isStomped = false;

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
        sprite.Draw(spriteBatch, position);
    }

    public void Update(GameTime gameTime)
    {
        if (!isStomped)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.X += velocityX * deltaTime;

            //TURN GOOMBA AROUND WHEN IT HITS THE EDGE OF ITS PATROL AREA
            if (position.X <= leftBound || position.X >= rightBound)
            {
                velocityX = -velocityX;
            }
        }

        sprite.UpdateAnimation(gameTime);
    }

    // public void Stomp()
    // {
    //     isStomped = true;
    //     sprite.Play("Stomped");
    // }
}