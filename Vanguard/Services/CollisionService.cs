
using Microsoft.Xna.Framework;

namespace MyGameProjectComplete.Services
{
    public static class CollisionService
    {
        public static bool CheckCollision(Rectangle rect1, Rectangle rect2)
        {
            return rect1.Intersects(rect2);
        }
    }
}
