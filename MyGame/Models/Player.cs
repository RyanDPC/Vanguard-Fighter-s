using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MyGame.Game;
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
        private int _screenWidth;
        private int _screenHeight;
        private Weapon _currentWeapon; // L'arme actuelle du joueur
        private Vector2 _weaponOffset = new Vector2(30, 50);
        public Player(ContentManager content, Texture2D texture, Vector2 initialPosition, int screenWidth, int screenHeight, int mapWidth, int mapHeight, Weapon weapon)
        {
            Position = initialPosition;
            this._texture = texture;
            this._screenWidth = screenWidth;
            this._screenHeight = screenHeight;
             _currentWeapon = weapon;
        }

        public void Update(GameTime gameTime, InputManager inputManager, TiledMap map)
        {
            ApplyGravity(gameTime);
            HandleInput(inputManager);
            CheckCollisions(map);
            UpdatePosition(gameTime);
            PreventLeavingScreen();
           
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
            isOnGround = false;
            Rectangle playerRect = new Rectangle((int)Position.X, (int)(Position.Y + _velocity.Y), PlayerWidth, PlayerHeight);

            foreach (var layer in map.TileLayers)
            {
                foreach (var tile in layer.Tiles)
                {
                    if (tile.GlobalIdentifier != 0)
                    {
                        Rectangle tileRect = new Rectangle(tile.X * map.TileWidth, tile.Y * map.TileHeight, map.TileWidth, map.TileHeight);
                        if (playerRect.Intersects(tileRect) && _velocity.Y > 0)
                        {
                            Position = new Vector2(Position.X, tileRect.Top - PlayerHeight);
                            _velocity.Y = 0;
                            isOnGround = true;
                        }
                    }
                }
            }
        }

        private void PreventLeavingScreen()
        {
            if (Position.X < 0) Position = new Vector2(0, Position.Y);
            else if (Position.X + PlayerWidth > _screenWidth) Position = new Vector2(_screenWidth - PlayerWidth, Position.Y);
            if (Position.Y + PlayerHeight > _screenHeight) Position = new Vector2(Position.X, _screenHeight - PlayerHeight);
            if (Position.Y < 0) Position = new Vector2(Position.X, 0);
        }

        private void UpdatePosition(GameTime gameTime)
        {
            Position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
private void UpdateWeapon(Vector2 mousePosition)
        {
            // Calculer l'angle entre le joueur et la souris
            Vector2 direction = mousePosition - (Position + _weaponOffset);
            float angle = (float)Math.Atan2(direction.Y, direction.X);

            // Définir la position et la rotation de l'arme
            _currentWeapon.SetRotation(angle);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, PlayerWidth, PlayerHeight);
            spriteBatch.Draw(_texture, destinationRectangle, Colour);
        }
    }
}
