using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        currentIndex = (currentIndex + 1) % enemies.Count;
    }

    public void Previous()
    {
        currentIndex = (currentIndex - 1 + enemies.Count) % enemies.Count;
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