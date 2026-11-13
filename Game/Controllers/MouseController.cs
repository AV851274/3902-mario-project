using Mario.Game.Commands;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Mario.Game.Controllers;

public class MouseController : IController
{
    private ICommand previousRoomCommand;
    private ICommand nextRoomCommand;
    private ICommand toggleHitboxesCommand;
    private MouseState previousState;
    public MouseController(IGameControls world)
    {
        previousRoomCommand = new PreviousRoomCommand(world);
        nextRoomCommand = new NextRoomCommand(world);
        toggleHitboxesCommand = new ToggleHitboxesCommand(world);

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

        if (mouseState.MiddleButton == ButtonState.Pressed && previousState.MiddleButton == ButtonState.Released)
        {
            toggleHitboxesCommand.Execute();
        }

        previousState = mouseState;
    }
}
