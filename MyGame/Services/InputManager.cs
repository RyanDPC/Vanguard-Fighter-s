using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MyGame.Services
{
    public class InputManager
    {
        private KeyboardState _currentKeyState;
        private KeyboardState _previousKeyState;

        public void Update()
        {
            // Mise à jour des états des touches
            _previousKeyState = _currentKeyState;
            _currentKeyState = Keyboard.GetState();
        }

        public Vector2 GetMovement()
        {
            Vector2 movement = Vector2.Zero;
            if (_currentKeyState.IsKeyDown(Keys.W)) movement.Y -= 1;
            if (_currentKeyState.IsKeyDown(Keys.S)) movement.Y += 1;
            if (_currentKeyState.IsKeyDown(Keys.A)) movement.X -= 1;
            if (_currentKeyState.IsKeyDown(Keys.D)) movement.X += 1;
            return movement;
        }

        public bool IsJumpPressed()
        {
            return _currentKeyState.IsKeyDown(Keys.Space) && _previousKeyState.IsKeyUp(Keys.Space);
        }

        public bool IsShootPressed()
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed;
        }

        public bool IsReloadPressed()
        {
            return _currentKeyState.IsKeyDown(Keys.R);
        }

        public bool IsWeaponSwitchPressed(int weaponNumber)
        {
            switch (weaponNumber)
            {
                case 1: return _currentKeyState.IsKeyDown(Keys.D1);
                case 2: return _currentKeyState.IsKeyDown(Keys.D2);
                default: return false;
            }
        }

        public bool IsEscapePressed()
        {
            return _currentKeyState.IsKeyDown(Keys.Escape) && _previousKeyState.IsKeyUp(Keys.Escape);
        }
     
    }
}
