using Mario.Interfaces.Player;
using Mario.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Mario.Controllers;

// TODO: mouse dash
public class MouseController : IController
{
    // private IPlayer player;

    public MouseController(IPlayer player)
    {
        // this.player = player;
    }

    public void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();

        // if (mouseState.LeftButton == ButtonState.Pressed)
        // {
        //     player.Dash();
        // }
    }
}
