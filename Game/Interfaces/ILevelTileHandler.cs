using Microsoft.Xna.Framework;

namespace Mario.Game.Interfaces;

internal interface ILevelTileHandler
{
    bool Place(string tile, Vector2 position);
}
