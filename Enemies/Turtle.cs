using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game2D.Interfaces;
using Game2D.Animation;
//IDENTICAL CODE FOR NOW, WILL BE BETTER WITH DEATH ANIMATION AND COLLISION DETECTION
public class Turtle : IEnemy
{
    private const float WalkSpeed = 60f;

    private ISprite sprite;
    private Vector2 position;
    private float velocityX = -WalkSpeed;
    private float leftBound;
    private float rightBound;
    private bool isStomped = false;

    private Direction facingDirection = Direction.Left;

    public Direction FacingDirection
    {
        get { return facingDirection; }
    }

    public Vector2 Position
    {
        get { return position; }
    }

    public Turtle(ISprite sprite, Vector2 initialPosition, int patrolDistance = 150)
    {
        this.sprite = sprite;
        position = initialPosition;
        leftBound = initialPosition.X - patrolDistance;
        rightBound = initialPosition.X + patrolDistance;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = facingDirection == Direction.Right
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;

            sprite.Draw(spriteBatch, position, effects: effects);
    }

    public void Update(GameTime gameTime)
    {
        if (!isStomped)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.X += velocityX * deltaTime;

            //TURN TURTLE AROUND WHEN IT HITS THE EDGE OF ITS PATROL AREA
            if (position.X <= leftBound || position.X >= rightBound)
            {
                velocityX = -velocityX;
                facingDirection = velocityX < 0 ? Direction.Left : Direction.Right;
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