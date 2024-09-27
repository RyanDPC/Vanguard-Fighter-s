
using Microsoft.Xna.Framework.Input;

namespace MyGameProjectComplete.Services
{
    public static class InputService
    {
        public static bool IsKeyPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }

        public static bool IsKeyReleased(Keys key)
        {
            return Keyboard.GetState().IsKeyUp(key);
        }
    }
}
