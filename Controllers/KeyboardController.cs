using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monogame;

public class KeyboardController : IController
{
    private IPlayer player;
    private Dictionary<Keys, ICommand> pressCommands;
    private KeyboardState previousState;

    public KeyboardController(Game1 game, IPlayer player, IItem item, IBlock block, EnemyCycler enemies)
    {
        this.player = player;

        pressCommands = new Dictionary<Keys, ICommand>
        {
            { Keys.T, new PreviousBlockCommand(block) },
            { Keys.Y, new NextBlockCommand(block) },
            { Keys.U, new PreviousItemCommand(item) },
            { Keys.I, new NextItemCommand(item) },
            { Keys.O, new PreviousEnemyCommand(enemies) },
            { Keys.P, new NextEnemyCommand(enemies) },
            { Keys.Q, new QuitCommand(game) },
            { Keys.R, new ResetCommand(game) },
        };

        previousState = Keyboard.GetState();
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        UpdateMovement(keyboardState);

        foreach (KeyValuePair<Keys, ICommand> pressCommand in pressCommands)
        {
            if (keyboardState.IsKeyDown(pressCommand.Key) && previousState.IsKeyUp(pressCommand.Key))
            {
                pressCommand.Value.Execute();
            }
        }

        previousState = keyboardState;
    }

    private void UpdateMovement(KeyboardState keyboardState)
    {
        bool left = keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A);
        bool right = keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D);
        bool dash = keyboardState.IsKeyDown(Keys.Space);

        if (left == right)
        {
            player.StopMoving();
        }
        else if (left)
        {
            if (dash)
            {
                player.DashLeft();
            } else {
                player.MoveLeft();
            }
        }
        else
        {
            if (dash)
            {
                player.DashRight();
            } else {
                player.MoveRight();
            }
        }

        if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
        {
            player.Jump();
        }
    }
}
