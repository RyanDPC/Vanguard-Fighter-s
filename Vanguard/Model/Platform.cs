using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vanguard.Models
{
    public class Platform
    {
        public Rectangle Bounds { get; private set; }

        public Platform(Rectangle bounds)
        {
            Bounds = bounds;
        }

        // Method to draw the platform
        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, Bounds, Color.White);
        }
    }
}
