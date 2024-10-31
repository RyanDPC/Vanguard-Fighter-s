using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Models;
using MyGame.Services;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGame.Models
{
    public class Player
    {
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public int Health { get; private set; }
        public Weapon Weapon { get; private set; }
        public bool IsOnGround { get; private set; }

        private const float Gravity = 1500f;
        private const float JumpStrength = -600f;
        private const int MaxHealth = 100;
        private int screenWidth;
        private int screenHeight;
        private bool _isFacingRight;
        private List<Bullet> bullets = new List<Bullet>();
        private bool isReloading;
        private float lastShotTime;
        

        public Player(Vector2 initialPosition, int screenWidth, int screenHeight, Weapon initialWeapon)
        {
            Position = initialPosition;
            Velocity = Vector2.Zero;
            Health = MaxHealth;
            Weapon = initialWeapon;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            _isFacingRight = false; // Le joueur regarde par défaut à gauche
            IsOnGround = false;
            isReloading = false;
            lastShotTime = 0f;
        }

        public void Update(GameTime gameTime, InputManager inputManager, TiledMap map)
        {
            ApplyGravity(gameTime);
            HandleMovement(inputManager);
            CheckCollisions(map);
            UpdatePosition(gameTime);
            UpdateBullets(gameTime);

            // Gestion du tir
            if (inputManager.IsShootPressed() && gameTime.TotalGameTime.TotalSeconds - lastShotTime >= 1 / Weapon.FireRate)
            {
                Shoot();
                lastShotTime = (float)gameTime.TotalGameTime.TotalSeconds;
            }

            // Gestion du rechargement
            if (inputManager.IsReloadPressed())
            {
                StartReloading(gameTime);
            }

            // Gestion de la capacité spéciale
            if (inputManager.IsSpecialAbilityPressed())
            {
                Weapon.ActivateSpecialAbility();
            }
        }

        private void ApplyGravity(GameTime gameTime)
        {
            if (!IsOnGround)
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        private void HandleMovement(InputManager inputManager)
        {
            Vector2 movement = inputManager.GetMovement();
            Velocity = new Vector2(movement.X * 300, Velocity.Y);

            // Changer l'orientation en fonction des touches A et D
            if (movement.X > 0) _isFacingRight = true;
            else if (movement.X < 0) _isFacingRight = false;

            // Gestion du saut
            if (inputManager.IsJumpPressed() && IsOnGround)
            {
                Velocity = new Vector2(Velocity.X, JumpStrength);
                IsOnGround = false;
            }
        }

        private void CheckCollisions(TiledMap map)
        {
            IsOnGround = false;
            Rectangle playerRect = new Rectangle((int)Position.X, (int)Position.Y, 64, 128);

            foreach (var layer in map.TileLayers)
            {
                foreach (var tile in layer.Tiles)
                {
                    if (tile.GlobalIdentifier != 0)
                    {
                        Rectangle tileRect = new Rectangle(tile.X * map.TileWidth, tile.Y * map.TileHeight, map.TileWidth, map.TileHeight);

                        if (playerRect.Intersects(tileRect) && Velocity.Y > 0)
                        {
                            Position = new Vector2(Position.X, tileRect.Top - playerRect.Height);
                            Velocity = new Vector2(Velocity.X, 0);
                            IsOnGround = true;
                            return;
                        }
                    }
                }
            }
        }

        private void UpdatePosition(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Position.X < 0) Position = new Vector2(0, Position.Y);
            if (Position.X > screenWidth - 64) Position = new Vector2(screenWidth - 64, Position.Y);
            if (Position.Y > screenHeight - 128) Position = new Vector2(Position.X, screenHeight - 128);
        }

        // Méthode pour tirer avec l'arme équipée
        public void Shoot()
        {
            Vector2 direction = _isFacingRight ? Vector2.UnitX : -Vector2.UnitX;
            Vector2 bulletVelocity = direction * Weapon.Speed;
            Bullet bullet = new Bullet(Position, bulletVelocity, Weapon.Damage, _isFacingRight, Weapon.WeaponOffset, Weapon.Speed);
            bullets.Add(bullet);
        }

        public List<Bullet> GetBullets()
        {
            return bullets;
        }
        // Démarrer le rechargement
        private void StartReloading(GameTime gameTime)
        {
            if (!isReloading)
            {
                isReloading = true;
                Task.Delay((int)(Weapon.ReloadTime * 1000)).ContinueWith(_ => FinishReloading(gameTime));
            }
        }

        private void FinishReloading(GameTime gameTime)
        {
            Weapon.Reload(gameTime);
            isReloading = false;
        }

        private void UpdateBullets(GameTime gameTime)
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update(gameTime);

                if (bullets[i].IsOffScreen(screenWidth, screenHeight))
                {
                    bullets.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effect = _isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Weapon.Texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);

            foreach (var bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

        public void ChangeWeapon(Weapon newWeapon)
        {
            Weapon = newWeapon;
            isReloading = false;
        }

        public void TakeDamage(int damage)
        {
            Health -= Weapon.Damage;
            if (Health <= 0)
            {
                Health = 0;
                Die();
            }
        }

        private async void Die()
        {
            Console.WriteLine("Player is dead.");
            await Task.Delay(10000);
            Environment.Exit(0);
        }
    }
}
