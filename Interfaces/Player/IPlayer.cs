using Mario.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mario.Interfaces.Player;

public interface IPlayer: IGameObject
{
    IPlayerBodyAction BodyAction { get; }
    IMarioPowerState MarioPowerState { get; }
}
