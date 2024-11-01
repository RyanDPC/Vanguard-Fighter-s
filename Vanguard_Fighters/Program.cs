using System;

namespace Vanguard_Fighters
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new Game1();
            game.Run();
        }
    }

    public enum GameState
    {
        MainMenu,
        Playing,
        GameOver,
        GameWin
    }
}
