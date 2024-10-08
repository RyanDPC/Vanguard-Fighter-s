using MyGame.Library;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame.Models
{
    public class Weapon
    {
        public string Name { get; private set; }
        public int Damage { get; private set; }
        public int MaxAmmo { get; private set; }
        public int ClipSize { get; private set; }
        public int CurrentAmmo { get; private set; }
        public float FireRate { get; private set; }
        public float ReloadTime { get; private set; }
        public string SpecialAbility { get; private set; }
        public Texture2D WeaponTexture { get; private set; }
        public Vector2 Size { get; private set; }
        public float Range { get; private set; }
        public bool IsReloading { get; private set; }

        private float timeSinceLastShot;
        private float reloadTimer;
        private float timeSinceLastSpecial;
        private float specialCooldown;
        private float _rotation;
        private SpriteEffects _spriteEffect;

        public Weapon(string name, int damage, float fireRate, int maxAmmo, int clipSize, float range, float reloadTime, string specialAbility, Texture2D texture)
        {
            Name = name;
            Damage = damage;
            FireRate = fireRate;
            MaxAmmo = maxAmmo;
            ClipSize = clipSize;
            CurrentAmmo = maxAmmo; // Set current ammo to max on init
            Range = range;
            ReloadTime = reloadTime;
            SpecialAbility = specialAbility;
            WeaponTexture = texture;
            Size = new Vector2(10, 6); // Default size, can be adjusted as necessary
            specialCooldown = 5.0f; // Default cooldown for special abilities
            timeSinceLastSpecial = specialCooldown;

            IsReloading = false;
            timeSinceLastShot = 0;
            reloadTimer = 0;
        }
        public void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (IsReloading)
            {
                reloadTimer += elapsedSeconds;
                if (reloadTimer >= ReloadTime)
                {
                    CurrentAmmo = ClipSize;
                    IsReloading = false;
                    reloadTimer = 0;
                }
            }
            else
            {
                timeSinceLastShot += elapsedSeconds;
            }

            // Update time since last special ability usage
            timeSinceLastSpecial += elapsedSeconds;
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
                Console.WriteLine($"{Name} shoots, remaining ammo: {CurrentAmmo}");
            }
        }
        public void Reload()
        {
            if (!IsReloading && CurrentAmmo < MaxAmmo)
            {
                IsReloading = true;
                reloadTimer = 0;
                Console.WriteLine($"{Name} is reloading...");
            }
        }
        public void UseSpecialAbility()
        {
            if (timeSinceLastSpecial >= specialCooldown)
            {
                timeSinceLastSpecial = 0;
                Console.WriteLine($"{Name} uses special ability: {SpecialAbility}");
                // Trigger special ability effects here
            }
        }
        public void SetRotation(float rotation, bool flipHorizontally)
        {
            SpriteEffects spriteEffect = flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            _rotation = rotation;
            _spriteEffect = spriteEffect;
        }
        public void SetWeaponTexture(Texture2D texture)
        {
            WeaponTexture = texture;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool isFacingRight, float scale = 0.1f)
        {
            Vector2 origin = new Vector2(WeaponTexture.Width / 2f, WeaponTexture.Height / 2f);
            SpriteEffects spriteEffect = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            // Dessiner l'arme avec la rotation et le flip selon la direction du joueur
            spriteBatch.Draw(WeaponTexture, position, null, Color.White, _rotation, origin, scale, spriteEffect, 0f);
        }
    }
}
