using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGame.Models
{
    public class Enemy
    {
        private Texture2D _texture;
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public int Health { get; set; } = 5;
        public bool IsAlive => Health > 0;

        private float speed = 100.0f; // Vitesse de l'ennemi
        private const int EnemyWidth = 64;
        private const int EnemyHeight = 128;

        private float gravity = 980f; // Gravité appliquée sur l'ennemi
        private bool isOnGround = false;
        private double _rotation;

        public Enemy(Texture2D texture, Vector2 initialPosition)
        {
            _texture = texture;
            Position = initialPosition;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                // L'ennemi meurt
                Die();
            }
        }

        private async void StartGameCloseCountdown()
        {
            await Task.Delay(5000); // Attendre 5 secondes
            Environment.Exit(0); // Ferme le jeu
        }

        public void Die()
        {
            Console.WriteLine("Enemy is dead.");
            StartGameCloseCountdown();
        }

        // Méthode pour dessiner l'ennemi
        public void Draw(Texture2D texture, SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                spriteBatch.Draw(texture, Position, Color.White);
            }
        }

        public Rectangle GetEnemyRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, EnemyWidth, EnemyHeight);
        }

        // Méthode pour déplacer l'ennemi en direction du joueur avec l'IA
        public void MoveTowardsPlayer(Vector2 playerPosition, GameTime gameTime, GraphicsDevice graphicsDevice, TiledMap tiledMap)
        {
            // Calculer la direction vers le joueur
            var direction = new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation));

            // Suivre le joueur uniquement sur l'axe X
            if (Math.Abs(direction.X) > 1f)
            {
                direction.Normalize(); // Normaliser pour obtenir une vitesse constante
                Position += new Vector2(direction.X * speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            }

            // Appliquer la gravité si l'ennemi n'est pas au sol
            if (!isOnGround)
            {
                // Recréer un nouveau Vector2 pour modifier la vélocité
                Velocity = new Vector2(Velocity.X, Velocity.Y + gravity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            Position += new Vector2(0, Velocity.Y); // Appliquer la gravité sur l'axe Y

            // Gérer les collisions avec les plateformes
            HandleCollisions(tiledMap);

            // Limiter les mouvements de l'ennemi aux dimensions de l'écran
            int screenWidth = graphicsDevice.Viewport.Width;
            int screenHeight = graphicsDevice.Viewport.Height;

            Position = new Vector2(
                MathHelper.Clamp(Position.X, 0, screenWidth - EnemyWidth),
                MathHelper.Clamp(Position.Y, 0, screenHeight - EnemyHeight)
            );
        }

        // Gérer les collisions avec les plateformes sur la carte Tiled
        private void HandleCollisions(TiledMap tiledMap)
        {
            isOnGround = false; // Réinitialiser l'état du sol

            foreach (var layer in tiledMap.TileLayers)
            {
                foreach (var tile in layer.Tiles)
                {
                    // Si la tuile est solide (c'est-à-dire qu'elle a un GlobalIdentifier)
                    if (tile.GlobalIdentifier > 0)
                    {
                        Rectangle tileRect = new Rectangle(
                            tile.X * tiledMap.TileWidth,
                            tile.Y * tiledMap.TileHeight,
                            tiledMap.TileWidth,
                            tiledMap.TileHeight
                        );

                        // Si l'ennemi touche une tuile solide
                        if (GetEnemyRectangle().Intersects(tileRect))
                        {
                            if (Velocity.Y > 0) // Si l'ennemi tombe (mouvement vers le bas)
                            {
                                // Recréer un nouveau Vector2 pour modifier la position
                                Position = new Vector2(Position.X, tileRect.Top - EnemyHeight);
                                isOnGround = true; // L'ennemi est maintenant au sol

                                // Recréer un nouveau Vector2 pour annuler la vélocité verticale
                                Velocity = new Vector2(Velocity.X, 0);
                            }
                        }
                    }
                }
            }
        }

        // Méthode pour dessiner l'ennemi
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                Rectangle destinationRectangle = new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    EnemyWidth,
                    EnemyHeight
                );

                spriteBatch.Draw(_texture, destinationRectangle, Color.White);
            }
        }
    }
}
