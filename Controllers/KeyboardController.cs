using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class KeyboardController : IController
{
    private IPlayer player;
    private Game game;

    private IItem item;
    private IBlock block;
    private KeyboardState previousState;

    public KeyboardController(Game game, IPlayer player, IItem item, IBlock block)
    {
        this.game = game;
        this.player = player;
        this.item = item;
        this.block = block;
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
        {
            player.MoveLeft();
        }
        else if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
        {
            player.MoveRight();
        }
        else
        {
            player.StopMoving();
        }
        if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
        {
            player.Jump();
        }
        if (keyboardState.IsKeyDown(Keys.Escape))
        {
            game.Exit();
        }
        if (keyboardState.IsKeyDown(Keys.O) && previousState.IsKeyUp(Keys.O))
        {
            item.nextSprite();
        }
        if (keyboardState.IsKeyDown(Keys.P) && previousState.IsKeyUp(Keys.P))
        {
            item.prevSprite();
        }
        if (keyboardState.IsKeyDown(Keys.K) && previousState.IsKeyUp(Keys.K))
        {
            block.nextSprite();
        }
        if (keyboardState.IsKeyDown(Keys.L) && previousState.IsKeyUp(Keys.L))
        {
            block.prevSprite();
        }

        previousState = keyboardState;
    }
}
