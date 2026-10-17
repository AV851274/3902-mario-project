using Mario.Game.Interfaces.Player;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Mario.Game.Controllers;

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
