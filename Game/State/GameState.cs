using Mario.Game.Interfaces.Player;
using Mario.Game.Interfaces;
using System.Collections.Generic;


namespace Mario.Game.State;

public class GameState
{
    public IPlayer Player { get; set; }

    public List<IBlock> Blocks { get; } = new();
    public List<IItem> Items { get; } = new();
    public List<IEnemy> Enemies { get; } = new();
    public List<IProjectile> Projectiles { get; } = new();
    public List<IGameObject> Background { get; } = new();

    public void Reset()
    {
        Player = null;
        Blocks.Clear();
        Items.Clear();
        Enemies.Clear();
        Projectiles.Clear();
        Background.Clear();
    }
}
