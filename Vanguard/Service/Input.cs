using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Timers;
using System;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using Vanguard.Menu;

namespace Vanguard.Service
{
    public class InputManager
    {
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        private GamePadState _currentGamePadState;
        private GamePadState _previousGamePadState;

        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        public event Action OnToggleFullScreen;

        // Call this method at the beginning of each frame
        public void Update(GameTime gameTime)
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _previousGamePadState = _currentGamePadState;
            _currentGamePadState = GamePad.GetState(PlayerIndex.One);

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            if (_currentKeyboardState.IsKeyDown(Keys.F12) && _previousKeyboardState.IsKeyUp(Keys.F12))
            {
                OnToggleFullScreen?.Invoke();
            }
        }

        // Get movement input as a Vector2
        public Vector2 GetMovement()
        {
            float movementX = 0f;

            // Keyboard input
            if (_currentKeyboardState.IsKeyDown(Keys.Left) || _currentKeyboardState.IsKeyDown(Keys.A))
            {
                movementX -= 1f;
            }
            if (_currentKeyboardState.IsKeyDown(Keys.Right) || _currentKeyboardState.IsKeyDown(Keys.D))
            {
                movementX += 1f;
            }

            // GamePad input (left thumbstick)
            movementX += _currentGamePadState.ThumbSticks.Left.X;

            return new Vector2(movementX, 0f);
        }

        // Check if the jump button is pressed
        public bool IsJumpPressed()
        {
            return _currentKeyboardState.IsKeyDown(Keys.Space);
        }

        // Check if the shoot button is pressed
        public bool IsShootPressed()
        {
            if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                Console.WriteLine("Shoot button pressed!");
                return true;
            }
            return false;
        }

        // Check if the reload button is pressed
        public bool IsReloadPressed()
        {
            if (_currentKeyboardState.IsKeyDown(Keys.R) && _previousKeyboardState.IsKeyUp(Keys.R))
            {
                return true;
            }
            return false;
        }
    }
}