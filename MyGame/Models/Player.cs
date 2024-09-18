using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MyGame.Services;

namespace MyGame.Models
{
    public class Player
    {
        public Vector2 Position { get; private set; }
        public Color Colour = Color.White;
        private Vector2 _velocity;
        private Texture2D _texture;
        private bool isOnGround = false;
        private float gravity = 1500f;
        private float jumpStrength = -600f; // Force du saut
        private const int PlayerWidth = 64;
        private const int PlayerHeight = 128;
        private int screenWidth;
        private int screenHeight;
        // Constructeur modifié pour inclure la taille de l'écran
        public Player(Texture2D texture, Vector2 initialPosition, int screenWidth, int screenHeight, int mapWidth, int mapHeight)
        {
            _texture = texture;
            Position = initialPosition;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        public void Update(GameTime gameTime, InputManager inputManager, TiledMap map)
        {
            ApplyGravity(gameTime);
            HandleInput(inputManager);
            CheckCollisions(map);
            UpdatePosition(gameTime);
            PreventLeavingScreen(); // Nouvelle méthode pour empêcher de quitter l'écran
        }

        private void ApplyGravity(GameTime gameTime)
        {
            if (!isOnGround)
            {
                _velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        private void HandleInput(InputManager inputManager)
        {
            if (isOnGround && inputManager.IsJumpPressed())
            {
                _velocity.Y = jumpStrength;
                isOnGround = false;
            }

            Vector2 movement = inputManager.GetMovement();
            _velocity.X = movement.X * 300; // Vitesse horizontale
        }

        private void CheckCollisions(TiledMap map)
        {
            // Gérer les collisions avec les tuiles du niveau
            isOnGround = false;

            Rectangle playerRect = new Rectangle((int)Position.X, (int)(Position.Y + _velocity.Y), PlayerWidth, PlayerHeight);

            foreach (var layer in map.TileLayers)
            {
                foreach (var tile in layer.Tiles)
                {
                    if (tile.GlobalIdentifier != 0) // Si ce n'est pas une tuile vide
                    {
                        Rectangle tileRect = new Rectangle(tile.X * map.TileWidth, tile.Y * map.TileHeight, map.TileWidth, map.TileHeight);

                        if (playerRect.Intersects(tileRect))
                        {
                            if (_velocity.Y > 0) // Si le joueur tombe
                            {
                                Position = new Vector2(Position.X, tileRect.Top - PlayerHeight);
                                _velocity.Y = 0;
                                isOnGround = true;
                            }
                        }
                    }
                }
            }
        }

        private void PreventLeavingScreen()
        {
            
            // Empêcher le joueur de dépasser les bords de l'écran
            if (Position.X < 0)
            {
                Position = new Vector2(0, Position.Y); // Bord gauche de l'écran
            }
            else if (Position.X + PlayerWidth > screenWidth)
            {
                Position = new Vector2(screenWidth - PlayerWidth, Position.Y); // Bord droit de l'écran
            }

            if (Position.Y + PlayerHeight > screenHeight)
            {
                Position = new Vector2(Position.X, screenHeight - PlayerHeight); // Bord bas de l'écran
                _velocity.Y = 0; // Stopper la chute
                isOnGround = true; // Le joueur est au sol
            }
            if (Position.Y < 0){
                Position = new Vector2(Position.X, 0);
            }
        }

        private void UpdatePosition(GameTime gameTime)
        {
            Position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle Positions = new Rectangle((int)Position.X, (int)Position.Y, PlayerWidth, PlayerHeight);
            spriteBatch.Draw(_texture, Positions, Colour);
        }
    }
}
