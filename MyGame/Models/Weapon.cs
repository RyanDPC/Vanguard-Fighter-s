using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MyGame.Models
{
    public class Weapon
    {
        public string Name { get; private set; }
        public int MaxAmmo { get; private set; }
        public int CurrentAmmo { get; private set; }
        public float FireRate { get; private set; } // Nombre de tirs par seconde
        public float ReloadTime { get; private set; } // Temps de rechargement en secondes
        public bool IsReloading { get; private set; }
        private float timeSinceLastShot;
        private float reloadTimer;
        private Texture2D _weaponTexture;

        private float _rotation; // Rotation de l'arme

        public Weapon(string name, int maxAmmo, float fireRate, int ammoCapacity, float reloadTime, string specialAbility)
        {
            Name = name;
            MaxAmmo = ammoCapacity;
            FireRate = fireRate;
            ReloadTime = reloadTime;
            CurrentAmmo = MaxAmmo;
            IsReloading = false;
            timeSinceLastShot = 0;
            reloadTimer = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (IsReloading)
            {
                reloadTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (reloadTimer >= ReloadTime)
                {
                    CurrentAmmo = MaxAmmo;
                    IsReloading = false;
                    reloadTimer = 0;
                }
            }
            else
            {
                timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
 public bool CanShoot()
        {
            return !IsReloading && CurrentAmmo > 0 && timeSinceLastShot >= (1 / FireRate);
        }
        public void Shoot()
        {
            if (CanShoot())
            {
                CurrentAmmo--;
                timeSinceLastShot = 0;
                Console.WriteLine($"{Name} tire, munitions restantes : {CurrentAmmo}");
            }
        }

        public void Reload()
        {
            if (!IsReloading)
            {
                IsReloading = true;
                reloadTimer = 0;
                Console.WriteLine($"{Name} est en train de recharger...");
            }
        }

        public void SetRotation(float rotation)
        {
            _rotation = rotation;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            // Définir les dimensions souhaitées pour l'arme
            int weaponWidth = _weaponTexture.Width / 4; 
            int weaponHeight = _weaponTexture.Height / 4;
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, weaponWidth, weaponHeight);

            // Dessiner l'arme avec rotation
            spriteBatch.Draw(_weaponTexture, destinationRectangle, null, Color.White, _rotation, new Vector2(weaponWidth / 2, weaponHeight / 2), SpriteEffects.None, 0f);
        }

        public void SetWeaponTexture(Texture2D texture)
        {
            _weaponTexture = texture;
        }

       
    }
}
