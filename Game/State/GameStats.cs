using System;
using Microsoft.Xna.Framework;

namespace Mario.Game.State;

public class GameStats
{
    private const int StartingLives = 3;
    private const float StartingTime = 400f;
    private const float SecondsPerTimeUnit = 0.4f;
    private const int CoinsPerExtraLife = 100;
    private const int CoinScore = 200;
    private const string StartingWorld = "1-1";

    public int Score { get; private set; }
    public int Coins { get; private set; }
    public int Lives { get; private set; }
    public float Time { get; private set; }
    public string World { get; set; }

    public bool IsOutOfTime => Time <= 0f;

    public GameStats()
    {
        Reset();
    }

    public void Reset()
    {
        Score = 0;
        Coins = 0;
        Lives = StartingLives;
        Time = StartingTime;
        World = StartingWorld;
    }

    public void Update(GameTime gameTime)
    {
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Time = Math.Max(0f, Time - elapsed / SecondsPerTimeUnit);
    }

    public void AddScore(int points)
    {
        Score += points;
    }

    public void AddCoin()
    {
        Coins++;
        Score += CoinScore;
        if (Coins >= CoinsPerExtraLife)
        {
            Coins -= CoinsPerExtraLife;
            Lives++;
        }
    }

    public void LoseLife()
    {
        Lives--;
        Time = StartingTime;
    }
}
