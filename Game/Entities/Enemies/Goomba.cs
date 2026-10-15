using Mario.Interfaces;
using Mario.Physics;
using Mario.Physics.Aspects;
using Mario.Physics.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Enemies;

public class Goomba : IEnemy
{
    private const float WalkSpeed = 60f;

    private ISprite sprite;
    private float rotation = 0f;

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

    public Goomba(ISprite sprite, Vector2 initialPosition, int patrolDistance = 150)
    {
        this.sprite = sprite;
        physicsState = new PhysicsState(initialPosition, Vector2.Zero);
        horizontalMovement = new HorizontalVelocityPhysics(-WalkSpeed);
        gravityPhysics = new GravityPhysics(gravity: 1000f) { Enabled = false };
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
        sprite.Draw(spriteBatch, physicsState.Position, rotation);
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
        } else
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
        rotation = MathHelper.ToRadians(180f);
        isStomped = true;
        gravityPhysics.Enabled = true;
    }
}
