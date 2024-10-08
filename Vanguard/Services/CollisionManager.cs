using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame.Services
{
    public class CollisionManager
    {
        public void ConstrainToScreen(Rectangle playerRectangle, GraphicsDevice graphicsDevice, ref Vector2 position)
        {
            int screenWidth = graphicsDevice.Viewport.Width;
            int screenHeight = graphicsDevice.Viewport.Height;

            if (position.X < 0)
            {
                position.X = 0;
            }
            else if (position.X > screenWidth - playerRectangle.Width)
            {
                position.X = screenWidth - playerRectangle.Width;
            }

            if (position.Y < 0)
            {
                position.Y = 0;
            }
            else if (position.Y > screenHeight - playerRectangle.Height)
            {
                position.Y = screenHeight - playerRectangle.Height;
            }
        }
    }
}
