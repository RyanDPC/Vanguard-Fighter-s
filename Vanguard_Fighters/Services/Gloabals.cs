using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame.Services
{
    public static class Globals
    {
        public static float Time {get; set; }
        public static ContentManager ContentManager {get; set;}
        public static SpriteBatch spriteBatch {get; set;}
        public static GraphicsDevice GraphicsDevice {get; set;} 

        public static Point WindowsSize {get; set;}

        public static void Update(GameTime gameTime)
        {
            Time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}