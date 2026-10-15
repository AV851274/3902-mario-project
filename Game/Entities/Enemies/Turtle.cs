using Mario.Animation;
using Mario.Interfaces;
using Mario.Physics;
using Mario.Physics.Aspects;
using Mario.Physics.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Enemies;

//IDENTICAL CODE FOR NOW, WILL BE BETTER WITH DEATH ANIMATION AND COLLISION DETECTION
public class Turtle : IEnemy
{
    private const float WalkSpeed = 60f;

    private ISprite sprite;
    private readonly PhysicsController physicsController;
    private readonly HorizontalVelocityPhysics horizontalMovement;
    private PhysicsState physicsState;
    private float leftBound;
    private float rightBound;
    private bool isStomped = false;

    private bool isKicked = false;

    private Direction facingDirection = Direction.Left;

    public Direction FacingDirection
    {
        get { return facingDirection; }
    }

    public Vector2 Position
    {
        get { return physicsState.Position; }
    }

    public Turtle(ISprite sprite, Vector2 initialPosition, int patrolDistance = 150)
    {
        this.sprite = sprite;
        physicsState = new PhysicsState(initialPosition, Vector2.Zero);
        horizontalMovement = new HorizontalVelocityPhysics(-WalkSpeed);
        physicsController = new PhysicsController
        {
            HorizontalVelocityPhysics = horizontalMovement,
            MovementPhysics = new MovementPhysics()
        };
        leftBound = initialPosition.X - patrolDistance;
        rightBound = initialPosition.X + patrolDistance;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = facingDirection == Direction.Right
            ? SpriteEffects.FlipHorizontally
            : SpriteEffects.None;

            sprite.Draw(spriteBatch, physicsState.Position, effects: effects);
    }

    public void Update(GameTime gameTime)
    {
        if (!isStomped || isKicked)
        {
            //TURN TURTLE AROUND WHEN IT HITS THE EDGE OF ITS PATROL AREA
            if (physicsState.Position.X <= leftBound || physicsState.Position.X >= rightBound)
            {
                horizontalMovement.VelocityX = -horizontalMovement.VelocityX;
                facingDirection = horizontalMovement.VelocityX < 0
                    ? Direction.Left
                    : Direction.Right;
            }
        }

        if (isStomped && !isKicked)
            horizontalMovement.VelocityX = 0f;
        else if (isKicked && horizontalMovement.VelocityX == 0f)
            horizontalMovement.VelocityX = 4 * WalkSpeed *
                (facingDirection == Direction.Left ? -1 : 1);

        physicsController.Update(ref physicsState, (float)gameTime.ElapsedGameTime.TotalSeconds);
        sprite.UpdateAnimation(gameTime);
    }

    public void Stomp()
    {
        if (!isStomped)
        {
            isStomped = true;
            sprite.Play("Stomped");
        } else if (!isKicked) {
            isKicked = true;
            horizontalMovement.VelocityX =
                4 * WalkSpeed * (facingDirection == Direction.Left ? -1 : 1);
        } else
        {
            isKicked = false;
        }
    }
}
