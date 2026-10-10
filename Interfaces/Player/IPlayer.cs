using Mario.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mario.Interfaces.Player;

public interface IPlayer: IGameObject
{
    IPlayerAction Action { get; }
    IMarioPowerState MarioPowerState { get; }
}
