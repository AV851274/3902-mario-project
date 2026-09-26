using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monogame;

public class KeyboardController : IController
{
    private IPlayer player;
    private Dictionary<Keys, Action> pressActions;
    private KeyboardState previousState;

    public KeyboardController(Game1 game, IPlayer player, IItem item, IBlock block, EnemyCycler enemies)
    {
        this.player = player;

        // Keys that trigger once per press
        pressActions = new Dictionary<Keys, Action>
        {
            { Keys.T, block.prevSprite },
            { Keys.Y, block.nextSprite },
            { Keys.U, item.prevSprite },
            { Keys.I, item.nextSprite },
            { Keys.O, enemies.Previous },
            { Keys.P, enemies.Next },
            { Keys.Q, game.Exit },
            { Keys.R, game.ResetGame },
        };

        // Start from the real keyboard state so a key held during reset isn't treated as a new press
        previousState = Keyboard.GetState();
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        UpdateMovement(keyboardState);

        foreach (KeyValuePair<Keys, Action> pressAction in pressActions)
        {
            if (keyboardState.IsKeyDown(pressAction.Key) && previousState.IsKeyUp(pressAction.Key))
            {
                pressAction.Value();
            }
        }

        previousState = keyboardState;
    }

    // Keys that act every frame while held
    private void UpdateMovement(KeyboardState keyboardState)
    {
        bool left = keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A);
        bool right = keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D);
        bool dash = keyboardState.IsKeyDown(Keys.Space);

        if (left == right) // neither or both pressed
        {
            player.StopMoving();
        }
        else if (left)
        {
            if (dash) player.DashLeft(); else player.MoveLeft();
        }
        else
        {
            if (dash) player.DashRight(); else player.MoveRight();
        }

        if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
        {
            player.Jump();
        }
    }
}
