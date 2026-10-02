namespace Mario;

public static class Program
{
    public static void Main()
    {
        using var game = new MarioGame();
        game.Run();
    }
}
