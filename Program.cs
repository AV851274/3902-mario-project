namespace Mario;

internal static class Program
{
    private static void Main()
    {
        using var game = new MarioGame();
        game.Run();
    }
}
