using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Mario.Game.Levels;

internal static class CsvLevelReader
{
    public static List<string[]> Read(ContentManager content, string fileName)
    {
        using var reader = new StreamReader(
            TitleContainer.OpenStream(Path.Combine(content.RootDirectory, "levels", fileName)));

        List<string[]> rows = [];
        while (reader.ReadLine() is { } line)
        {
            rows.Add(line.Split(','));
        }
        return rows;
    }
}
