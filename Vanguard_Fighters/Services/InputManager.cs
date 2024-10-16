using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Drawing;

namespace MyGame.Services
{
    public class InputManager
    {
        private KeyboardState _currentKeyState;
        private KeyboardState _previousKeyState;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private GraphicsDeviceManager _graphics;

        public void Update()
        {
            // Mise à jour des états des touches
            _previousKeyState = _currentKeyState;
            _currentKeyState = Keyboard.GetState();

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
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
            return _currentMouseState.LeftButton == ButtonState.Pressed;
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
                case 3: return _currentKeyState.IsKeyDown(Keys.D3);
                case 4: return _currentKeyState.IsKeyDown(Keys.D4);
                case 5: return _currentKeyState.IsKeyDown(Keys.P);
                case 6: return _currentKeyState.IsKeyDown(Keys.D6);
                case 7: return _currentKeyState.IsKeyDown(Keys.D7);
                case 8: return _currentKeyState.IsKeyDown(Keys.D8);
                case 9: return _currentKeyState.IsKeyDown(Keys.D9);

                default: return false;
            }
        }
        public bool IsSpecialAbilityPressed()
        {
            return _currentMouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton==ButtonState.Released;
        }
        public bool IsEscapePressed()
        {
          
            return _currentKeyState.IsKeyDown(Keys.Escape) && _previousKeyState.IsKeyUp(Keys.Escape);
        }
     
    }
}
