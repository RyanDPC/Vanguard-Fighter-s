using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Vanguard.Models
{
    public class Weapon
    {
        // Weapon properties
        public string Name { get; }
        public int Damage { get; }
        public float FireRate { get; }
        public int AmmoCapacity { get; }
        public float ReloadTime { get; }
        public float InitialReloadCooldown { get; }
        public float InitialFireCooldown { get; }
        public float BulletSpeed { get; } // Nouvelle propriété : vitesse des balles
        public float Range { get; } // Nouvelle propriété : portée
        public Texture2D Texture { get; }

        // Internal fields
        private int currentAmmo;
        private float reloadCooldown;
        private float fireCooldown;
        private bool isReloading;

        public Weapon(string name, int damage, float fireRate, int ammoCapacity, float reloadTime,
                     float initialReloadCooldown, float initialFireCooldown, float bulletSpeed, float range, Texture2D texture)
        { 
            Name = name;
            Damage = damage;
            FireRate = fireRate;
            AmmoCapacity = ammoCapacity;
            ReloadTime = reloadTime;
            InitialReloadCooldown = initialReloadCooldown;
            InitialFireCooldown = initialFireCooldown;
            BulletSpeed = bulletSpeed; 
            Range = range; 
            Texture = texture;

            currentAmmo = ammoCapacity;
            reloadCooldown = InitialReloadCooldown;
            fireCooldown = InitialFireCooldown;
            isReloading = false;
        }

        public bool Shoot(GameTime gameTime, Vector2 weaponPosition, Vector2 direction, List<Bullet> bullets)
        {
            // Vérifier les conditions empêchant le tir
            if (isReloading)
            {
                Console.WriteLine($"Cannot shoot: Weapon is reloading. Ammo={currentAmmo}");
                return false;
            }
            if (fireCooldown > 0f)
            {
                Console.WriteLine($"Cannot shoot: Fire cooldown is active ({fireCooldown}s).");
                return false;
            }
            if (currentAmmo <= 0)
            {
                Console.WriteLine($"Cannot shoot: No ammo left.");
                return false;
            }

            // Tirer une balle
            fireCooldown = 1f / FireRate;
            currentAmmo--;

            Bullet bullet = new Bullet(weaponPosition, direction * BulletSpeed, Damage, Range);
            bullets.Add(bullet);

            Console.WriteLine($"Bullet fired: Position={weaponPosition}, Velocity={bullet.Velocity}, AmmoLeft={currentAmmo}");
            return true;
        }


        public void Reload()
        {
            if (!isReloading && currentAmmo < AmmoCapacity)
            {
                isReloading = true;
                reloadCooldown = ReloadTime;
            }
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Gestion du rechargement
            if (isReloading)
            {
                HandleReloading(deltaTime);
            }
            else if (currentAmmo <= 0)
            {
                Reload(); // Rechargement automatique si les munitions sont épuisées
            }

            // Gestion du cooldown de tir
            if (fireCooldown > 0f)
            {
                fireCooldown -= deltaTime;
            }
        }

        private void HandleReloading(float deltaTime)
        {
            reloadCooldown -= deltaTime;

            if (reloadCooldown <= 0)
            {
                currentAmmo = AmmoCapacity; // Remplir le chargeur
                isReloading = false;        // Fin du rechargement
                Console.WriteLine("Weapon reloaded.");
            }
        }


        public int GetCurrentAmmo()
        {
            return currentAmmo;
        }

        public bool IsReloading()
        {
            return isReloading;
        }
    }
}
