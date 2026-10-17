using Mario.Game.Interfaces;
using Mario.Game.Physics;
using Mario.Game.Physics.Aspects;
using Mario.Game.Physics.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Enemies;

public class Goomba : IEnemy
{
    private const float WalkSpeed = 60f;
    private const int Size = 32;

    private ISprite sprite;

    private readonly PhysicsController physicsController;
    private readonly HorizontalVelocityPhysics horizontalMovement;
    private readonly GravityPhysics gravityPhysics;
    private PhysicsState physicsState;
    private float leftBound;
    private float rightBound;
    private bool isStomped;

    public Vector2 Position
    {
        get { return physicsState.Position; }
    }

    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Size, Size);
    public bool IsDefeated => isStomped;
    public bool CanHurtPlayer => !isStomped;

    public Goomba(ISprite sprite, Vector2 initialPosition, int patrolDistance = 150)
    {
        this.sprite = sprite;
        physicsState = new PhysicsState(initialPosition, Vector2.Zero);
        horizontalMovement = new HorizontalVelocityPhysics(-WalkSpeed);
        gravityPhysics = new GravityPhysics(gravity: 1000f);
        physicsController = new PhysicsController
        {
            HorizontalVelocityPhysics = horizontalMovement,
            GravityPhysics = gravityPhysics,
            MovementPhysics = new MovementPhysics()
        };
        leftBound = initialPosition.X - patrolDistance;
        rightBound = initialPosition.X + patrolDistance;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects = isStomped ? SpriteEffects.FlipVertically : SpriteEffects.None;
        sprite.Draw(spriteBatch, Position, effects: effects);
    }

    public void Update(GameTime gameTime)
    {
        if (!isStomped)
        {
            //TURN GOOMBA AROUND WHEN IT HITS THE EDGE OF ITS PATROL AREA
            if (physicsState.Position.X <= leftBound || physicsState.Position.X >= rightBound)
            {
                horizontalMovement.VelocityX = -horizontalMovement.VelocityX;
            }
        }
        else
        {
            horizontalMovement.VelocityX = 0f;
        }

        physicsController.Update(ref physicsState, (float)gameTime.ElapsedGameTime.TotalSeconds);
        sprite.UpdateAnimation(gameTime);
    }

    public void Stomp()
    {
        if (isStomped)
            return;
        
        sprite.Play("Stomped");
        isStomped = true;
        horizontalMovement.VelocityX = 0f;
    }

    public void LandOn(float groundTop)
    {
        if (isStomped)
            return;

        physicsState.Position = new Vector2(physicsState.Position.X, groundTop - Size);
        physicsState.Velocity = new Vector2(physicsState.Velocity.X, 0f);
        physicsState.IsOnGround = true;
    }

    public void BounceOffWall(float newX)
    {
        physicsState.Position = new Vector2(newX, physicsState.Position.Y);
        horizontalMovement.VelocityX = -horizontalMovement.VelocityX;
    }
}
