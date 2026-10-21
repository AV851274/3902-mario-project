using System;
using System.Collections;
using Mario.Game.Interfaces.Player;
using Mario.Game.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;


namespace Mario.Game.State;

public class GameEntities
{
    public List<IPlayer> Players { get; } = new();
    public List<IBlock> Blocks { get; } = new();
    public List<IItem> Items { get; } = new();
    public List<IEnemy> Enemies { get; } = new();
    public List<IProjectile> Projectiles { get; } = new();
    public List<IGameObject> Background { get; } = new();

    public IEnumerable<IGameObject> GetAllGameObjects()
    {
        return Background
            .Concat(Items)
            .Concat(Blocks)
            .Concat(Enemies)
            .Concat(Projectiles)
            .Concat(Players);
    }

    public void Update(GameTime gameTime)
    {
        Players.RemoveAll(p => !p.Active);
        Blocks.RemoveAll(b => !b.Active);
        Items.RemoveAll(i => !i.Active);
        Enemies.RemoveAll(e => !e.Active);
        Projectiles.RemoveAll(p => !p.Active);
        Background.RemoveAll(b => !b.Active);

        foreach (var obj in GetAllGameObjects())
        {
            obj.Update(gameTime);
        }
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