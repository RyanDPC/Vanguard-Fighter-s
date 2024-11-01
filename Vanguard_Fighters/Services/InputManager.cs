using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyGame.Models;
using System;

namespace MyGame.Services
{
    public class InputManager
    {
        private KeyboardState _currentKeyState;
        private KeyboardState _previousKeyState;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private bool isFacingRight = true;
        private float timeSinceLastShot;
        private float fireCooldown;

        public void Update(GameTime gameTime)
        {
            // Met à jour le temps écoulé depuis le dernier tir en secondes pour plus de précision
            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Mise à jour des états des touches
            _previousKeyState = _currentKeyState;
            _currentKeyState = Keyboard.GetState();

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }

        public Vector2 GetMovement()
        {
            Vector2 velocity = Vector2.Zero;
            const float speed = 1f;

            if (_currentKeyState.IsKeyDown(Keys.W)) velocity.Y -= speed;
            if (_currentKeyState.IsKeyDown(Keys.S)) velocity.Y += speed;

            // Vérifie l'appui sur A ou D pour l'orientation
            if (_currentKeyState.IsKeyDown(Keys.A))
            {
                velocity.X -= speed;
                isFacingRight = false; // Le joueur regarde à gauche
            }
            else if (_currentKeyState.IsKeyDown(Keys.D))
            {
                velocity.X += speed;
                isFacingRight = true; // Le joueur regarde à droite
            }

            return velocity;
        }

        public bool IsFacingRight() => isFacingRight;

        public bool IsJumpPressed()
        {
            return _currentKeyState.IsKeyDown(Keys.Space) && _previousKeyState.IsKeyUp(Keys.Space);
        }

        public bool IsShootPressed()
        {
            // Vérifie si le bouton est pressé et si le cooldown est terminé
            if (_currentMouseState.LeftButton == ButtonState.Pressed && timeSinceLastShot >= fireCooldown)
            {
                // Réinitialise le temps depuis le dernier tir pour appliquer le cooldown
                timeSinceLastShot = 0f;
                Console.WriteLine("Tir effectué"); // Debug
                return true;
            }
            return false;
        }

        public bool IsReloadPressed()
        {
            return _currentKeyState.IsKeyDown(Keys.R);
        }

        public bool IsWeaponSwitchPressed(int weaponNumber)
        {
            return weaponNumber switch
            {
                1 => _currentKeyState.IsKeyDown(Keys.Z),
                2 => _currentKeyState.IsKeyDown(Keys.X),
                3 => _currentKeyState.IsKeyDown(Keys.C),
                4 => _currentKeyState.IsKeyDown(Keys.V),
                5 => _currentKeyState.IsKeyDown(Keys.B),
                6 => _currentKeyState.IsKeyDown(Keys.N),
                7 => _currentKeyState.IsKeyDown(Keys.M),
                8 => _currentKeyState.IsKeyDown(Keys.K),
                9 => _currentKeyState.IsKeyDown(Keys.L),
                _ => false,
            };
        }


        public bool IsSpecialAbilityPressed()
        {
            return _currentMouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton == ButtonState.Released;
        }

        public bool IsEscapePressed()
        {
            return _currentKeyState.IsKeyDown(Keys.Escape) && _previousKeyState.IsKeyUp(Keys.Escape);
        }

        public void SetFireCooldown(Weapon weapon)
        {
            if (weapon != null)
            {
                fireCooldown = weapon.FireRate; // Utilise le FireRate de l'arme pour définir le cooldown
                Console.WriteLine("fireCooldown défini sur : " + fireCooldown); // Debug
            }
            else
            {
                Console.WriteLine("Erreur : Arme non définie !");
            }
        }
    }
}
