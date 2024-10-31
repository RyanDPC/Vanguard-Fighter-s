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
        private const float SPEED = 1f;
        private bool isFacingRight = true;

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
            Vector2 _velocity = Vector2.Zero;
            if (_currentKeyState.IsKeyDown(Keys.W)) _velocity.Y -= SPEED;
            if (_currentKeyState.IsKeyDown(Keys.S)) _velocity.Y += SPEED;

            // Vérifie l'appui sur A ou D pour l'orientation
            if (_currentKeyState.IsKeyDown(Keys.A))
            {
                _velocity.X -= SPEED;
                isFacingRight = false; // Le joueur regarde à gauche
            }
            else if (_currentKeyState.IsKeyDown(Keys.D))
            {
                _velocity.X += SPEED;
                isFacingRight = true; // Le joueur regarde à droite
            }

            return _velocity;
        }

        public bool IsFacingRight()
        {
            return isFacingRight;
        }
    

    public bool IsJumpPressed()
        {
            return _currentKeyState.IsKeyDown(Keys.Space) && _previousKeyState.IsKeyUp(Keys.Space);
        }

        public bool IsShootPressed()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released;
        }

        public bool IsReloadPressed()
        {
            return _currentKeyState.IsKeyDown(Keys.R);
        }

        public bool IsWeaponSwitchPressed(int weaponNumber)
        {
            switch (weaponNumber)
            {
                case 1: return _currentKeyState.IsKeyDown(Keys.NumPad1);
                case 2: return _currentKeyState.IsKeyDown(Keys.NumPad2);
                case 3: return _currentKeyState.IsKeyDown(Keys.NumPad3);
                case 4: return _currentKeyState.IsKeyDown(Keys.NumPad4);
                case 5: return _currentKeyState.IsKeyDown(Keys.NumPad5);
                case 6: return _currentKeyState.IsKeyDown(Keys.NumPad6);
                case 7: return _currentKeyState.IsKeyDown(Keys.NumPad7);
                case 8: return _currentKeyState.IsKeyDown(Keys.NumPad8);
                case 9: return _currentKeyState.IsKeyDown(Keys.NumPad9);

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
