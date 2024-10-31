using Microsoft.Xna.Framework;
using MyGame.Models;
using MyGame.Services;
using MonoGame.Extended.Tiled;
using System;
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
        }

        // Mise à jour de la position et des mouvements
        public void Update(GameTime gameTime, InputManager inputManager, TiledMap map)
        {
            ApplyGravity(gameTime);
            HandleMovement(inputManager);
            CheckCollisions(map);
            UpdatePosition(gameTime);
        }

        private void ApplyGravity(GameTime gameTime)
        {
            if (!IsOnGround)
            {
                Velocity += new Vector2(0, Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds);
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

            _isFacingRight = movement.X >= 0; // Mise à jour de la direction du joueur
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
                            Position = new Vector2(Position.X, tileRect.Top - 128); // Corrige la position pour éviter de traverser le sol
                            Velocity = new Vector2(Velocity.X, 0); // Arrête le mouvement vertical
                            IsOnGround = true;
                        }
                    }
                }
            }
        }

        private void UpdatePosition(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Empêcher le joueur de sortir de l'écran
            if (Position.X < 0) Position = new Vector2(0, Position.Y);
            if (Position.X > screenWidth - 64) Position = new Vector2(screenWidth - 64, Position.Y);
            if (Position.Y > screenHeight - 128) Position = new Vector2(Position.X, screenHeight - 128);
        }

        // Méthode pour tirer avec l'arme équipée
        public void Shoot(Texture2D bulletTexture)
        {
            // Position actuelle du joueur
            Vector2 position = Position;

            // Exemple de direction (vers la droite)
            Vector2 direction = new Vector2(1, 0);

            Weapon.Shoot(bulletTexture, position, direction);
        }

        // Méthode pour changer d'arme
        public void ChangeWeapon(Weapon newWeapon)
        {
            Weapon = newWeapon;
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
            Console.WriteLine("Enemy is dead.");
            await Task.Delay(10000); // Attendre 5 secondes
            Environment.Exit(0);
        }
    }
}
