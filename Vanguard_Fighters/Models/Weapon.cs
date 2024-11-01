using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vanguard_Fighters.Library;

namespace MyGame.Models
{
    public class Weapon
    {
        private WeaponStats _weaponStats;
        private Texture2D WeaponTexture => _weaponStats.WeaponTexture;
        private int _currentAmmo;
        private bool _isReloading;
        private float _lastShotTime;
        private Texture2D _bulletTexture;
        private Vector2 _weaponOffset;
        private float _scaleFactor;
        private List<Bullet> _bullets;
        public Weapon(WeaponStats weaponStats, Texture2D bulletTexture, Vector2 weaponOffset, float scaleFactor)
        {
           
            _weaponStats = weaponStats;
            _bulletTexture = bulletTexture;
            _weaponOffset = weaponOffset;
            _scaleFactor = scaleFactor;
            _currentAmmo = _weaponStats.ClipSize;
            _bullets = new List<Bullet>();
            _isReloading = false;
        }

        // Changer d'arme
        public void SwitchWeapon(WeaponStats newWeaponStats)
        {
            _weaponStats = newWeaponStats;
            _currentAmmo = _weaponStats.ClipSize;
            _isReloading = false;
            _lastShotTime = 0;
        }

        // Gestion du tir
        public bool TryShoot(GameTime gameTime, Vector2 position, bool isFacingRight)
        {
            if (_isReloading || _currentAmmo <= 0) return false;

            float timeSinceLastShot = (float)gameTime.TotalGameTime.TotalSeconds - _lastShotTime;
            if (timeSinceLastShot < _weaponStats.FireRate) return false;

            _lastShotTime = (float)gameTime.TotalGameTime.TotalSeconds;
            _currentAmmo--;

            // Calculer la direction et position de tir en fonction de l'orientation
            Vector2 direction = isFacingRight ? Vector2.UnitX : -Vector2.UnitX;
            Vector2 bulletStartPosition = position + new Vector2(_weaponOffset.X * (isFacingRight ? 1 : -1), _weaponOffset.Y) * _scaleFactor;

            Shoot(bulletStartPosition, direction);
            return true;
        }

        // Fonction de tir
        private void Shoot(Vector2 position, Vector2 direction)
        {
            Bullet bullet = new Bullet(position, direction * _weaponStats.Speed * _scaleFactor, _weaponStats.Damage, _bulletTexture, _scaleFactor);
            _bullets.Add(bullet);
        }
        public void UpdateBullets(GameTime gameTime, int screenWidth, int screenHeight)
        {
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                _bullets[i].Update(gameTime); // Supposons que la classe Bullet a une méthode Update

                // Retirez les balles qui sortent de l'écran
                if (_bullets[i].IsOffScreen(screenWidth, screenHeight)) // Méthode IsOffScreen à ajouter dans la classe Bullet
                {
                    _bullets.RemoveAt(i);
                }
            }
        }

        public void DrawBullets(SpriteBatch spriteBatch)
        {
            foreach (var bullet in _bullets)
            {
                bullet.Draw(spriteBatch); // Supposons que la classe Bullet a une méthode Draw
            }
        }
        // Fonction de rechargement
        public void Reload(GameTime gameTime)
        {
            if (_isReloading || _currentAmmo == _weaponStats.ClipSize) return;

            _isReloading = true;
            float reloadTime = _weaponStats.ReloadTime;

            Task.Delay((int)(reloadTime * 1000)).ContinueWith(_ =>
            {
                _currentAmmo = _weaponStats.ClipSize;
                _isReloading = false;
            });
        }

        // Capacité spéciale
        public void ActivateSpecialAbility()
        {
            Console.WriteLine($"Activating special ability: {_weaponStats.Ability}");
            // Placeholder pour une capacité spéciale - afficher le texte dans la console
        }

        // Dessiner l'arme avec le facteur d'échelle
        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool isFacingRight)
        {
            var texture = _weaponStats.WeaponTexture;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

            // Ajuster la position avec offset et échelle en fonction de l'orientation
            Vector2 drawPosition = position + new Vector2(_weaponOffset.X * (isFacingRight ? 1 : -1), _weaponOffset.Y) * _scaleFactor;

            spriteBatch.Draw(
                texture,
                drawPosition,
                null,
                Color.White,
                0f,
                origin,
                _scaleFactor, // Application de l'échelle
                isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0f
            );
        }
    }
}
