using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MyGame.Services;

namespace MyGame.Models
{
    public class Player
    {
        public Vector2 Position { get; private set; }
        private Vector2 _velocity;
        private Texture2D _texture;
        private bool isOnGround = false;
        private float gravity = 1500f;
        private float jumpStrength = -600f; // Force du saut
        private const int PlayerWidth = 64;
        private const int PlayerHeight = 128;

        public Player(Texture2D texture, Vector2 initialPosition)
        {
            _texture = texture;
            Position = initialPosition;
        }

        public void Update(GameTime gameTime, InputManager inputManager, TiledMap map)
        {
            ApplyGravity(gameTime);
            HandleInput(inputManager);
            CheckCollisions(map);
            UpdatePosition(gameTime);
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

        private void UpdatePosition(GameTime gameTime)
        {
            Position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, PlayerWidth, PlayerHeight);
            spriteBatch.Draw(_texture, destinationRectangle, Color.White);
        }
    }
}
