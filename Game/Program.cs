using System;

namespace My2DGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new Microsoft.Xna.Framework.Game();
            game.Run();
        }
    }
}
