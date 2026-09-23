using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class KeyboardController : IController
{
    private IPlayer player;
    private Game game;

    public KeyboardController(Game game, IPlayer player)
    {
        this.player = player;
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Left))
        {
            player.MoveLeft();
        }
        else if (keyboardState.IsKeyDown(Keys.Right))
        {
            player.MoveRight();
        }
        else
        {
            player.StopMoving();
        }
        if (keyboardState.IsKeyDown(Keys.Up))
        {
            player.Jump();
        }
        if (keyboardState.IsKeyDown(Keys.Escape))
        {
            game.Exit();
        }
    }
}