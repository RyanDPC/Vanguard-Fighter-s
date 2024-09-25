using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame.Models
{
    public class Weapon
    {
        public string Name { get; private set; }
        public int MaxAmmo { get; private set; }
        public int CurrentAmmo { get; private set; }
        public float FireRate { get; private set; } // Shots per second
        public float ReloadTime { get; private set; } // Reload time in seconds
        public bool IsReloading { get; private set; }
        public float Scale { get; private set; } = 0.1f;
        public Texture2D WeaponTexture => _weaponTexture;

        private float timeSinceLastShot;
        private float reloadTimer;
        public Vector2 Size;
        private Texture2D _weaponTexture;
        private string specialAbility;
        private float specialCooldown;
        private float timeSinceLastSpecial;
        private float _rotation;
        private SpriteEffects _spriteEffect;

        public Weapon(string name, int maxAmmo, float fireRate, int clipSize, float reloadTime, string specialAbility)
        {
            Name = name;
            MaxAmmo = clipSize;
            FireRate = fireRate;
            ReloadTime = reloadTime;
            CurrentAmmo = MaxAmmo;
            IsReloading = false;
            timeSinceLastShot = 0;
            reloadTimer = 0;
            Size = new Vector2(10, 6);
            this.specialAbility = specialAbility;
            specialCooldown = 5.0f; // Default cooldown for special abilities
            timeSinceLastSpecial = specialCooldown;
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

            // Update time since last special ability usage
            timeSinceLastSpecial += (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            if (!IsReloading)
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

                switch (specialAbility)
                {
                    case "Automatic burst with slight accuracy loss":
                        Console.WriteLine($"{Name} triggers an automatic burst.");
                        break;
                    case "Fast reload":
                        Console.WriteLine($"{Name} reloads faster.");
                        ReloadTime *= 0.5f;
                        break;
                    case "Explosive shot every 5 shots":
                        Console.WriteLine($"{Name} triggers an explosive shot!");
                        break;
                    case "Overheats after 5 shots with increased damage":
                        Console.WriteLine($"{Name} overheats after multiple shots.");
                        break;
                    case "Temporarily disables enemy shields":
                        Console.WriteLine($"{Name} temporarily disables enemy shields.");
                        break;
                    case "Delayed explosion causing area damage":
                        Console.WriteLine($"{Name} triggers a delayed explosion.");
                        break;
                    case "Impulse that pushes nearby enemies":
                        Console.WriteLine($"{Name} pushes nearby enemies.");
                        break;
                    case "Silent shot without revealing position":
                        Console.WriteLine($"{Name} shoots silently.");
                        break;
                    case "Precision mode with extended range":
                        Console.WriteLine($"{Name} enters precision mode.");
                        break;
                    default:
                        Console.WriteLine($"{Name} uses an undefined special ability.");
                        break;
                }
            }
            else
            {
                Console.WriteLine($"{Name}'s special ability is on cooldown.");
            }
        }

        public void SetRotation(float rotation, bool flipHorizontally)
        {
            SpriteEffects spriteEffect = flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            _rotation = rotation;
            _spriteEffect = spriteEffect;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool isFacingRight, float Scale = 0.1f)
        {
            
            Vector2 origin = new Vector2(_weaponTexture.Width / 2f, _weaponTexture.Height / 2);
            // Appliquer le flip horizontal en fonction de la direction du joueur
            SpriteEffects spriteEffect = !isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            // Dessiner l'arme avec la dddbonne direction
            spriteBatch.Draw(_weaponTexture, position, null, Color.White, 0f, origin, Scale, spriteEffect, 0f);

        }
            public void SetWeaponTexture(Texture2D texture)
        {
            _weaponTexture = texture;
        }
    }
}
