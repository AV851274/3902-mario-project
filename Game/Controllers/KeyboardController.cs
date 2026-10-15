using Mario.Interfaces.Player;
using Mario;
using Mario.Animation;
using Mario.Commands;
using Mario.Enemies;
using Mario.Interfaces;
using Mario.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Mario.Controllers;

public class KeyboardController : IController
{
    private enum PlayerActions
    {
        MoveL,
        MoveR,
        Jump,
        DashL,
        DashR,
        StopMoving,
        Attack,
        Crouch
    }

    private Dictionary<Keys, ICommand> pressCommands;
    private Dictionary<PlayerActions, ICommand> playerCommands;
    private KeyboardState previousState;

    public KeyboardController(MarioGame game, IPlayer player, EnemyCycler enemies)
    {
        pressCommands = new Dictionary<Keys, ICommand> {
            { Keys.O, new PreviousEnemyCommand(enemies) },
            { Keys.P, new NextEnemyCommand(enemies) },
            { Keys.K, new StompEnemyCommand(enemies) },
            { Keys.Q, new QuitCommand(game) },
            { Keys.R, new ResetCommand(game) },
            { Keys.E, new PlayerTakeDamageCommand(player) },
            { Keys.D1, new ChangeMarioCommand(game, false, MarioPower.Small) },
            { Keys.D2, new ChangeMarioCommand(game, false, MarioPower.Big) },
            { Keys.D3, new ChangeMarioCommand(game, false, MarioPower.Fire) },
            { Keys.D4, new ChangeMarioCommand(game, true, MarioPower.Small) },
            { Keys.D5, new ChangeMarioCommand(game, true, MarioPower.Big) },
            { Keys.D6, new ChangeMarioCommand(game, true, MarioPower.Fire) },
        };

        playerCommands = new Dictionary<PlayerActions, ICommand> {
            { PlayerActions.MoveL, new PlayerMoveCommand(player, Direction.Left) },
            { PlayerActions.MoveR, new PlayerMoveCommand(player, Direction.Right) },
            { PlayerActions.Jump, new PlayerJumpCommand(player) },
            { PlayerActions.DashL, new PlayerDashCommand(player, Direction.Left) },
            { PlayerActions.DashR, new PlayerDashCommand(player, Direction.Right) },
            { PlayerActions.StopMoving, new PlayerStopMovingCommand(player) },
            { PlayerActions.Attack, new PlayerAttackCommand(player) },
            { PlayerActions.Crouch, new PlayerCrouchCommand(player) }
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

        PlayerActions action;

        if (left == right)
            action = PlayerActions.StopMoving;
        else if (left)
            action = dash ? PlayerActions.DashL : PlayerActions.MoveL;
        else
            action = dash ? PlayerActions.DashR : PlayerActions.MoveR;
        playerCommands[action].Execute();

        if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            playerCommands[PlayerActions.Jump].Execute();

        if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            playerCommands[PlayerActions.Crouch].Execute();

        if (keyboardState.IsKeyDown(Keys.F) && !previousState.IsKeyDown(Keys.F))
            playerCommands[PlayerActions.Attack].Execute();
    }
}
