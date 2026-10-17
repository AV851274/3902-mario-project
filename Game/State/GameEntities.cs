using Mario.Game.Interfaces.Player;
using Mario.Game.Interfaces;
using System.Collections.Generic;
using System.Linq;


namespace Mario.Game.State;

public class GameEntities
{
    public List<IPlayer> Players { get; set; } = new();
    public List<IBlock> Blocks { get; } = new();
    public List<IItem> Items { get; } = new();
    public List<IEnemy> Enemies { get; } = new();
    public List<IProjectile> Projectiles { get; } = new();
    public List<IGameObject> Background { get; } = new();

    public IEnumerable<IGameObject> GetAllGameObjects()
    {
        return Background
            .Concat(Players)
            .Concat(Blocks)
            .Concat(Items)
            .Concat(Enemies)
            .Concat(Projectiles);
    }

    public void Reset()
    {
        Players.Clear();
        Blocks.Clear();
        Items.Clear();
        Enemies.Clear();
        Projectiles.Clear();
        Background.Clear();
    }
}