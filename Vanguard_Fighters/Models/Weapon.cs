using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        private float lastShotTime;
        private bool isReloading;
        private float reloadStartTime;

        public Weapon(WeaponStats stats)
        {
            MaxAmmo = stats.MaxAmmo;
            Ammo = stats.ClipSize;
            Damage = stats.Damage;
            FireRate = stats.FireCooldown;
            Range = stats.Range;
            ReloadTime = stats.ReloadTime;
            SpecialAbility = stats.Ability;
            Texture = stats.WeaponTexture;
            lastShotTime = 0;
            isReloading = false;
        }

        public Bullet Shoot(Vector2 position, Vector2 direction, GameTime gameTime)
        {
            if (isReloading)
            {
                return null; // Ne pas tirer si en rechargement
            }

            if (Ammo > 0 && gameTime.TotalGameTime.TotalSeconds - lastShotTime >= 1 / FireRate)
            {
                lastShotTime = (float)gameTime.TotalGameTime.TotalSeconds;
                Ammo--;

                // Création du projectile avec les stats de l'arme
                float projectileSpeed = 500f;
                return new Bullet(Texture, position, direction * projectileSpeed, Damage);
            }
            return null; // Pas de tir si l'arme est en cours de rechargement ou sans munitions
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
                // Vérifier si le temps de rechargement est écoulé
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
                // Implémentez l'effet spécial ici
            }
            else if (SpecialAbility == "Fast reload")
            {
                // Exemple : Réduire le temps de rechargement temporairement
                // ReloadTime /= 2; (Note : ajustez selon votre logique)
            }
            // Ajoutez d'autres capacités spéciales selon l'attribut SpecialAbility
        }
    }
}
