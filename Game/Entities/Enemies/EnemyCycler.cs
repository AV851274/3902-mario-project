using Mario.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Mario.Entities.Enemies;

public class EnemyCycler
{
    private List<IEnemy> enemies;
    private int currentIndex = 0;

    public EnemyCycler(List<IEnemy> enemies)
    {
        this.enemies = enemies;
    }

    public void Next()
    {
        if (enemies.Count == 0)
        {
            currentIndex = 0;
            return;
        }

        currentIndex = (currentIndex + 1) % enemies.Count;
    }

    public void Previous()
    {
        if (enemies.Count == 0)
        {
            currentIndex = 0;
            return;
        }

        currentIndex = (currentIndex - 1 + enemies.Count) % enemies.Count;
    }

    public void StompCurrent()
    {
        if (enemies.Count == 0)
        {
            currentIndex = 0;
            return;
        }

        currentIndex %= enemies.Count;
        enemies[currentIndex].Stomp();
    }

    public void Update(GameTime gameTime)
    {
        enemies[currentIndex].Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        enemies[currentIndex].Draw(spriteBatch);
    }
}
