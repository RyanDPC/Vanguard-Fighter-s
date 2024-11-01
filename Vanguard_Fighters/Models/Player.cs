using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MyGame.Library;
using System;

namespace MyGame.Models
{
    public class PlayerModel
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; private set; } = Vector2.Zero;
        public int Health { get; private set; } = 100;
        public bool IsAlive => Health > 0;
        public bool IsFacingRight { get; private set; } = true;
        public bool IsOnGround { get; private set; }

        private float speed = 150.0f; // Vitesse du joueur
        private const int BaseWidth = 68; // Largeur du joueur
        private const int BaseHeight = 128; // Hauteur du joueur
        private float scaleFactor; // Facteur d'échelle
        private const float Gravity = 980f;

        public PlayerModel(Vector2 initialPosition, Weapon initialWeapon, float scaleFactor)
        {
            Position = initialPosition;
            _weapon = initialWeapon;
            this.scaleFactor = scaleFactor;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Console.WriteLine("Player is dead.");
            // Actions supplémentaires lors de la mort du joueur
        }

        public void Move(Vector2 direction, GameTime gameTime, TiledMap tiledMap)
        {
            if (direction.X != 0)
            {
                direction.Normalize();
                Position += new Vector2(direction.X * speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                IsFacingRight = direction.X > 0; // Met à jour l'orientation
            }

            // Appliquer la gravité
            if (!IsOnGround)
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            // Mise à jour de la position
            Position += new Vector2(0, Velocity.Y);

            // Gérer les collisions avec les tuiles
            HandleCollisions(tiledMap);
        }

        private void HandleCollisions(TiledMap tiledMap)
        {
            IsOnGround = false; // Réinitialiser l'état du sol
            Rectangle playerRect = GetPlayerRectangle();

            foreach (var layer in tiledMap.TileLayers)
            {
                foreach (var tile in layer.Tiles)
                {
                    if (tile.GlobalIdentifier > 0) // Si la tuile est solide
                    {
                        Rectangle tileRect = new Rectangle(
                            tile.X * tiledMap.TileWidth,
                            tile.Y * tiledMap.TileHeight,
                            tiledMap.TileWidth,
                            tiledMap.TileHeight
                        );

                        if (playerRect.Intersects(tileRect))
                        {
                            // Gestion des collisions en fonction de la direction du mouvement
                            if (Velocity.Y > 0) // Collision en tombant
                            {
                                Position = new Vector2(Position.X, tileRect.Top - GetScaledHeight());
                                IsOnGround = true; // Le joueur est maintenant au sol
                                Velocity = Vector2.Zero; // Réinitialise la vélocité
                            }
                            else if (Velocity.Y < 0) // Collision en sautant
                            {
                                Position = new Vector2(Position.X, tileRect.Bottom);
                                Velocity = new Vector2(Velocity.X, 0); // Annule la vélocité vers le haut
                            }

                            // Collision sur les côtés
                            if (Position.X < tileRect.Left)
                            {
                                Position = new Vector2(tileRect.Left - GetScaledWidth(), Position.Y);
                            }
                            else if (Position.X > tileRect.Right)
                            {
                                Position = new Vector2(tileRect.Right, Position.Y);
                            }
                        }
                    }
                }
            }
        }

        public Rectangle GetPlayerRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, GetScaledWidth(), GetScaledHeight());
        }

        private int GetScaledWidth()
        {
            return (int)(BaseWidth * scaleFactor);
        }

        private int GetScaledHeight()
        {
            return (int)(BaseHeight * scaleFactor);
        }
    }
}
