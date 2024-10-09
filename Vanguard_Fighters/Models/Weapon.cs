
using System;
using MyGame.Library;
using MyGame.View;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame.Models
{
    public class Weapon
    {
        public WeaponStats Stats { get; private set; } // Stocke les caractéristiques de l'arme
        public Texture2D WeaponTexture { get; private set; } // Texture de l'arme
        public List<Bullet> Bullets { get; private set; } // Liste des projectiles

        private float timeSinceLastShot; // Temps écoulé depuis le dernier tir
        private float reloadTimer; // Timer pour le rechargement
        private float timeSinceLastSpecial; // Temps écoulé depuis la dernière utilisation de la capacité spéciale
        private bool isReloading; // Indique si l'arme est en train de recharger

        public Weapon(WeaponStats stats, Texture2D texture)
        {

            Stats = stats;
            WeaponTexture = texture;
            Bullets = new List<Bullet>();
            timeSinceLastShot = 0f;
            reloadTimer = Stats.ReloadTime;
            timeSinceLastSpecial = Stats.FireCooldown;
            isReloading = false;
        }
        public void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Si l'arme est en cours de rechargement
            if (isReloading)
            {
                reloadTimer += elapsedSeconds;
                if (reloadTimer >= Stats.ReloadTime)
                {
                    Console.WriteLine($"{Stats.Name} is reloaded.");
                    isReloading = false;
                    reloadTimer = 0;
                }
            }
            timeSinceLastShot += elapsedSeconds;

            // Mise à jour du cooldown de la capacité spéciale
            timeSinceLastSpecial += elapsedSeconds;
        }
        public bool CanShoot()
        {
            return !isReloading && Stats.MaxAmmo > 0 && timeSinceLastShot >= (1 / Stats.Range);
        }

        public void Shoot(Texture2D bulletTexture, Vector2 position, Vector2 direction)
        {
            if (CanShoot())
            {
                // Crée un projectile
                Bullet bullet = new Bullet(bulletTexture, position, direction);
                Bullets.Add(bullet);

                // Réinitialise le cooldown du tir
                timeSinceLastShot = 0;
                Stats.MaxAmmo--;

                Console.WriteLine($"{Stats.Name} shoots. Remaining ammo: {Stats.MaxAmmo}");
            }
            else
            {
                Console.WriteLine($"{Stats.Name} cannot shoot. Either reloading or out of ammo.");
            }
        }

        public void Reload()
        {
            if (!isReloading && Stats.MaxAmmo < Stats.ClipSize)
            {
                isReloading = true;
                reloadTimer = 0;
                Console.WriteLine($"{Stats.Name} is reloading...");
            }
        }
        public void UseSpecialAbility()
        {
            if (timeSinceLastSpecial >= Stats.FireCooldown)
            {
                timeSinceLastSpecial = 0;
                Console.WriteLine($"{Stats.Name} uses special ability: {Stats.Ability}");
            }
            else
            {
                Console.WriteLine($"{Stats.Name} cannot use special ability yet.");
            }
        }
        public void SetWeaponTexture(Texture2D texture)
        {
            WeaponTexture = texture;
        }
        public void UpdateBullets(GameTime gameTime)
        {
            foreach (var bullet in Bullets)
            {
                bullet.Update(gameTime);
            }

            // Retirer les balles hors écran
            Bullets.RemoveAll(b => b.IsOffScreen());
        }
        public void DrawBullets(SpriteBatch spriteBatch)
        {
            foreach (var bullet in Bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }
    }
}
