using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Services;
using System;

namespace MyGame.Models
{
    public class Weapon
    {
        public int Ammo { get; private set; }
        public int MaxAmmo { get; }
        public int Damage { get; }
        public float FireRate { get; }
        public float Range { get; }
        public float ReloadTime { get; }
        public string SpecialAbility { get; }
        public Texture2D Texture { get; }
        public float Speed { get; }
        public Vector2 WeaponOffset { get; set; } // Ajout pour la position de départ des balles

        private float lastShotTime;
        private bool isReloading;
        private float reloadStartTime;
        private bool IsFacingRight;

        public Weapon(WeaponStats stats, Vector2 weaponOffset)
        {
            MaxAmmo = stats.MaxAmmo;
            Ammo = stats.ClipSize;
            Damage = stats.Damage;
            FireRate = stats.FireCooldown;
            Range = stats.Range;
            ReloadTime = stats.ReloadTime;
            SpecialAbility = stats.Ability;
            Texture = stats.WeaponTexture;
            Speed = stats.Speed;
            WeaponOffset = weaponOffset;
            lastShotTime = 0;
            isReloading = false;
        }

        public Bullet Shoot(Vector2 position, Vector2 direction, GameTime gameTime, Texture2D bulletTexture)
        {
            if (isReloading) return null;

            if (Ammo > 0 && gameTime.TotalGameTime.TotalSeconds - lastShotTime >= 1 / FireRate)
            {
                lastShotTime = (float)gameTime.TotalGameTime.TotalSeconds;
                Ammo--;

                // Ajouter WeaponOffset pour positionner la balle au bout de l'arme
                Vector2 bulletStartPosition = position + WeaponOffset * (IsFacingRight ? 1 : -1);
                Vector2 bulletVelocity = direction * Speed;

                return new Bullet(bulletStartPosition, bulletVelocity, Damage, IsFacingRight, WeaponOffset, Speed);
            }
            return null;
        }



        public void Reload(GameTime gameTime)
        {
            if (!isReloading)
            {
                isReloading = true;
                reloadStartTime = (float)gameTime.TotalGameTime.TotalSeconds;
            }
            else
            {
                if ((float)gameTime.TotalGameTime.TotalSeconds - reloadStartTime >= ReloadTime)
                {
                    Ammo = MaxAmmo;
                    isReloading = false;
                }
            }
        }

        public void ActivateSpecialAbility()
        {
            if (SpecialAbility == "Automatic burst with slight accuracy loss")
            {
                // Implémenter l'effet spécial ici
            }
            else if (SpecialAbility == "Fast reload")
            {
                // Exemple : Réduire temporairement le temps de rechargement
            }
        }
    }
}
