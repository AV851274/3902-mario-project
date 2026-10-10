using Mario.Animation;
using Mario.Blocks;
using Mario.Controllers;
using Mario.Enemies;
using Mario.Interfaces;
using Mario.Items;
using Mario.Players;
using Mario.Projectiles;
using Mario.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Mario.GameStates;

namespace Mario;

public class MarioGame : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private IPlayer player;

    private bool isSwimming;

    // private ISprite fireballSprite;
    private EnemyCycler enemies;
    private IController keyboardController;
    private IController mouseController;
    private SpriteFactory spriteFactory;
    private GameState gameState = new GameState();

    public MarioGame()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        spriteFactory = new SpriteFactory();
        spriteFactory.LoadTextures(Content);

        ResetGame();
    }


    public void ResetGame()
    {
        gameState.Reset();
        // fireballSprite = spriteFactory.CreateFireBallSprite();

        gameState.Items.Add(
            new Item(spriteFactory, ItemType.Mushroom, new Vector2(400, 200)));

        gameState.Blocks.Add(
            new Block(spriteFactory, BlockType.Question, new Vector2(250, 200)));

        var hammerBro = new HammerBro(
            spriteFactory.CreateHammerBroSprite(),
            new Vector2(600, 392));

        hammerBro.SummonHammer += SpawnHammer;

        var goomba = new Goomba(
            spriteFactory.CreateGoombaSprite(),
            new Vector2(600, 408));

        var turtle = new Turtle(
            spriteFactory.CreateTurtleSprite(),
            new Vector2(600, 392));

        gameState.Enemies.Add(goomba);
        gameState.Enemies.Add(turtle);
        gameState.Enemies.Add(hammerBro);

        enemies = new EnemyCycler(gameState.Enemies);

        CreateMario(false, new Vector2(100, 100));
    }


    // Keys 1-6: switch between regular and swimming Mario, then set his size
    public void ChangeMario(bool swimming, MarioPower power)
    {
        if (swimming != isSwimming)
        {
            CreateMario(swimming, player.Position);
        }

        switch (power)
        {
            case MarioPower.Small: player.BecomeSmall(); break;
            case MarioPower.Big: player.BecomeBig(); break;
            case MarioPower.Fire: player.BecomeFire(); break;
        }
    }

    private void CreateMario(bool swimming, Vector2 position)
    {
        isSwimming = swimming;
        player = swimming
            ? new SwimmingPlayer(spriteFactory, position)
            : new Player(spriteFactory, position);
        
        gameState.Player = player;

        player.SummonFireball += SpawnFireball;

        // Controllers hold on to the player, so they're rebuilt whenever Mario is replaced
        keyboardController = new KeyboardController(this, player, enemies);
        mouseController = new MouseController(player);
    }

    private void SpawnFireball(Vector2 position, Direction direction)
    {
        float flip = direction == Direction.Left ? -1f : 1f;
        var velocity = new Vector2(850f * flip, 0);
        gameState.Projectiles.Add(new Fireball(
            spriteFactory.CreateFireballSprite(), // new animation contoller for each fireball
            position + new Vector2(25 * flip, -50f),
            velocity,
            1f,
            1000f,
            400f
        ));
    }

    private void SpawnHammer(Vector2 position, Direction direction)
    {
        float directionSign = direction == Direction.Left ? -1f : 1f;

        gameState.Projectiles.Add(new Hammer(
            spriteFactory.CreateHammerSprite(),
            position + new Vector2(20f * directionSign, -20f),
            new Vector2(250f * directionSign, -350f),
            3f,
            1000f,
            600f
        ));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        keyboardController.Update(gameTime);
        //mouseController.Update(gameTime);
        player.Update(gameTime);
        foreach (IItem currentItem in gameState.Items){
            currentItem.Update(gameTime);
        }
        foreach (IBlock currentBlock in gameState.Blocks){
            currentBlock.Update(gameTime);
        }
        enemies.Update(gameTime);
        UpdateProjectiles(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

        player.Draw(spriteBatch);

        foreach (IItem currentItem in gameState.Items){
            currentItem.Draw(spriteBatch);
        }
        foreach (IBlock currentBlock in gameState.Blocks){
            currentBlock.Draw(spriteBatch);
        }
        enemies.Draw(spriteBatch);
        foreach (var projectile in gameState.Projectiles)
        {
            projectile.Draw(spriteBatch);
        }

        spriteBatch.End();

        base.Draw(gameTime);
    }

    private void UpdateProjectiles(GameTime gameTime)
    {
        foreach (var projectile in gameState.Projectiles)
        {
            if (projectile.Active)
                projectile.Update(gameTime);
        }

        gameState.Projectiles.RemoveAll(p => !p.Active);
    }
}
