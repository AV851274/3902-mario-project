using Mario.Game.Commands;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Mario.Game.Controllers;

public class MouseController : IController
{
    private ICommand previousRoomCommand;
    private ICommand nextRoomCommand;
    private MouseState previousState;

    public MouseController(MarioGame game)
    {
        previousRoomCommand = new PreviousRoomCommand(game);
        nextRoomCommand = new NextRoomCommand(game);

        // Start from the real mouse state so a click held during a room switch isn't treated as a new click
        previousState = Mouse.GetState();
    }

    public void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();

        if (mouseState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released)
        {
            previousRoomCommand.Execute();
        }

        if (mouseState.RightButton == ButtonState.Pressed && previousState.RightButton == ButtonState.Released)
        {
            nextRoomCommand.Execute();
        }

        previousState = mouseState;
    }
}
