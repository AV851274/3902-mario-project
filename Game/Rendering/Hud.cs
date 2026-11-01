using System;
using Mario.Game.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Rendering;

public class Hud
{
    private const float TopMargin = 16f;
    private const float RowGap = 4f;
    private static readonly string[] Labels = { "MARIO", "COINS", "WORLD", "TIME", "LIVES" };

    private readonly SpriteFont font;
    private readonly GameStats stats;

    public Hud(SpriteFont font, GameStats stats)
    {
        this.font = font;
        this.stats = stats;
    }

    public void Draw(SpriteBatch spriteBatch, int screenWidth)
    {
        string[] values =
        {
            stats.Score.ToString("D6"),
            "x" + stats.Coins.ToString("D2"),
            stats.World,
            ((int)Math.Ceiling(stats.Time)).ToString("D3"),
            "x" + stats.Lives
        };

        float columnWidth = screenWidth / (float)Labels.Length;
        float valueY = TopMargin + font.LineSpacing + RowGap;

        for (int i = 0; i < Labels.Length; i++)
        {
            float centerX = columnWidth * (i + 0.5f);
            DrawCentered(spriteBatch, Labels[i], centerX, TopMargin);
            DrawCentered(spriteBatch, values[i], centerX, valueY);
        }
    }

    private void DrawCentered(SpriteBatch spriteBatch, string text, float centerX, float y)
    {
        float width = font.MeasureString(text).X;
        spriteBatch.DrawString(font, text, new Vector2(centerX - width / 2f, y), Color.White);
    }
}
