using Microsoft.Xna.Framework;
using MyGame.Models;
using MyGame.Services;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

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
        private List<Bullet> bullets; // Liste des projectiles tirés
        private bool isReloading;

        public Player(Vector2 initialPosition, int screenWidth, int screenHeight, Weapon initialWeapon)
        {
            Position = initialPosition;
            Velocity = Vector2.Zero;
            Health = MaxHealth;
            Weapon = initialWeapon;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            _isFacingRight = true; // Le joueur regarde par défaut à droite
            IsOnGround = false;
            bullets = new List<Bullet>();
            isReloading = false;
        }

        // Mise à jour de la position, des mouvements et des tirs
        public void Update(GameTime gameTime, InputManager inputManager, TiledMap map)
        {
            ApplyGravity(gameTime);
            HandleMovement(inputManager);
            CheckCollisions(map);
            UpdatePosition(gameTime);
            UpdateBullets(gameTime); // Mise à jour des projectiles

            // Gestion du tir
            if (inputManager.IsShootPressed() && !isReloading)
            {
                Shoot(gameTime);
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
        public void Shoot(GameTime gameTime)
        {
            Vector2 direction = new Vector2(_isFacingRight ? 1 : -1, 0);
            Bullet bullet = Weapon.Shoot(Position, direction, gameTime);

            if (bullet != null)
            {
                bullets.Add(bullet); // Ajouter le projectile à la liste des projectiles
            }
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
                    bullets.RemoveAt(i); // Supprimer le projectile s'il est hors de l'écran
                }
            }
        }
        public List<Bullet> GetBullets()
        {
            return bullets;
        }

        // Dessine le joueur et ses projectiles
        public void Draw(SpriteBatch spriteBatch)
        {
            // Dessiner le joueur (utilisez une texture de joueur)
            spriteBatch.Draw(Weapon.Texture, Position, Color.White);

            // Dessiner les projectiles
            foreach (var bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

        // Méthode pour changer d'arme
        public void ChangeWeapon(Weapon newWeapon)
        {
            Weapon = newWeapon;
            isReloading = false; // Arrêter le rechargement si on change d'arme
        }

        // Méthode pour gérer les dégâts reçus
        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Health = 0;
                Die();
            }
        }

        // Gérer la mort du joueur
        private async void Die()
        {
            Console.WriteLine("Player is dead.");
            await Task.Delay(10000); // Attendre 10 secondes
            Environment.Exit(0);
        }
    }
}
