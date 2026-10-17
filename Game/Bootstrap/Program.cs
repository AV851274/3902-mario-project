namespace Mario.Bootstrap;

public static class Program
{
    public static void Main()
    {
        using var game = new MarioGame();
        game.Run();
    }
}
