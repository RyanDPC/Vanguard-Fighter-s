using Microsoft.Xna.Framework;

namespace Vanguard.Service
{
    public static class Collision
    {
        public static void ConstrainToScreen(Rectangle rect, int screenWidth, int screenHeight, ref Vector2 position, ref Vector2 velocity)
        {
            // Left boundary
            if (rect.Left < 0)
            {
                position.X = 0;
                velocity.X = 0;
            }
            // Right boundary
            if (rect.Right > screenWidth)
            {
                position.X = screenWidth - rect.Width;
                velocity.X = 0;
            }
            // Top boundary
            if (rect.Top < 0)
            {
                position.Y = 0;
                velocity.Y = 0;
            }
            // Bottom boundary
            if (rect.Bottom > screenHeight)
            {
                position.Y = screenHeight - rect.Height;
                velocity.Y = 0;
            }
        }

        // Collision fictive avec les plateformes pour empêcher la traversée
        public static void ConstrainToPlatform(Rectangle platformRect, Rectangle entityRect, ref Vector2 position, ref Vector2 velocity)
        {
            if (entityRect.Bottom > platformRect.Top && entityRect.Top < platformRect.Top && entityRect.Right > platformRect.Left && entityRect.Left < platformRect.Right && velocity.Y > 0)
            {
                // Empêcher l'entité de traverser la plateforme par le dessous
                position.Y = platformRect.Top - entityRect.Height;
                velocity.Y = 0;
            }
        }
    }
}