using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MyGame.Game;
using MyGame.Services;
using MyGame.Models;
using MyGame.Content.Weapons;

namespace MyGame.Models
{
    public class Player
    {
        private WeaponLibrary library;
        public Vector2 Position { get; private set; }
        public Color Colour = Color.White;
        private Vector2 _velocity;
        private Texture2D _texture;
        private bool isOnGround = false;
        private bool _isFacingRight = true; // Le joueur fait face à la droite par défaut
        private float gravity = 1500f;
        private float jumpStrength = -600f; // Force du saut
        private const int PlayerWidth = 64;
        private const int PlayerHeight = 128;
        private int _screenWidth;
        private int _screenHeight;
        private Weapon _currentWeapon; // L'arme actuelle du joueur
        private Vector2 _weaponOffset = new Vector2(30, 50); // Position de l'arme par rapport au joueur
        //Bullet
        public List<Bullet> Bullets { get; private set; } = new List<Bullet>();
        private float timeSinceLastShot = 0f;
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

            // Mise à jour du cooldown du tir
            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Vérifier si le joueur tire (touche de tir)

            if (inputManager.IsShootPressed())
            {
                Fire();
            }

            // Mise à jour des projectiles
            foreach (var bullet in Bullets)
            {
                bullet.Update(gameTime);
            }

            // Retirer les projectiles hors écran
            Bullets.RemoveAll(b => b.Bounds.X < 0 || b.Bounds.X > _screenWidth);

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
                                            // Si le joueur se déplace à gauche
            if (movement.X < 0)
            {
                _isFacingRight = true; // Le joueur regarde à gauche
            }
            // Si le joueur se déplace à droite
            else if (movement.X > 0)
            {
                _isFacingRight = false; // Le joueur regarde à droite
            }
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

        
        private void Fire()
        {
            if (timeSinceLastShot >= (1 / _currentWeapon.FireRate)) {
                _currentWeapon.Shoot();
                
                timeSinceLastShot = 0;
            }
            {
                // Créer un projectile (mini-rectangle)
                Vector2 bulletVelocity = !_isFacingRight ? new Vector2(600, 0) : new Vector2(-600, 0); // Vitesse du projectile
                Vector2 bulletStartPos = Position + (_isFacingRight ? _weaponOffset : new Vector2(-_weaponOffset.X, _weaponOffset.Y));

                Texture2D bulletTexture = _currentWeapon.WeaponTexture; // Texture du projectile depuis l'arme

                // Ajouter le projectile à la liste
                Bullets.Add(new Bullet(bulletTexture, bulletStartPos, bulletVelocity));

                timeSinceLastShot = 0f; // Réinitialiser le cooldown
            }
        }
        private void UpdateWeapon()
        {
            _currentWeapon.SetRotation(0f, _isFacingRight);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            // Dessiner le joueur avec effet de flip s'il fait face à gauche
            SpriteEffects spriteEffect = _isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, PlayerWidth, PlayerHeight);
            spriteBatch.Draw(_texture, destinationRectangle, null, Colour, 0f, Vector2.Zero, spriteEffect, 0f);

            // Ajuster la position de l'arme en fonction de la direction du joueur
            Vector2 weaponPosition = Position + (_isFacingRight ? _weaponOffset : new Vector2(-_weaponOffset.X, _weaponOffset.Y));

            // Dessiner l'arme avec la bonne direction
            Vector2 weaponOffset = _isFacingRight ? new Vector2(10,-10) : new Vector2(10,-10);
            _currentWeapon.Draw(spriteBatch, weaponPosition, _isFacingRight);

            // Dessiner les projectiles
            foreach (var bullet in Bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

    }
}
