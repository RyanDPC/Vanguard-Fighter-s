using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MyGame.Models;
using MyGame.View;
using MyGame.Services;
using MyGame.Library;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System.Collections.Generic;
using System;
using Vanguard_Fighters.Library;

namespace Vanguard_Fighters
{
    public class GameLogic
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TiledMapRenderer _mapRenderer;
        private PlayerModel _playerModel;
        private PlayerView _playerView;
        private EnemyLibrary _enemyLibrary;
        private WeaponsLibrary _weaponsLibrary;
        private InputManager _inputManager;
        private List<Bullet> _bullets;
        private List<TiledMap> _maps;
        private Texture2D _currentBackground;

        public GameLogic(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
            _bullets = new List<Bullet>();
            _maps = new List<TiledMap>();
        }

        public void LoadContent(ContentManager content)
        {
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
            _mapRenderer = new TiledMapRenderer(_graphics.GraphicsDevice);

            // Load Player Components
            _playerModel = new PlayerModel();
            _playerView = new PlayerView(content);

            // Load Enemy Library
            _enemyLibrary = new EnemyLibrary(content);

            // Load Weapon Library
            _weaponsLibrary = new WeaponsLibrary();

            // Load Maps and Backgrounds
            var map = content.Load<TiledMap>("Textures/TheForest");
            _maps.Add(map);
            _mapRenderer.LoadMap(map);

            _currentBackground = content.Load<Texture2D>("Maps/Background");

            // Initialize Input Manager
            _inputManager = new InputManager();
        }

        public void Update(GameTime gameTime)
        {
            // Handle Player Input
            _inputManager.Update();
            _playerModel.Update(_inputManager, gameTime);

            // Update Enemies
            _enemyLibrary.Update(gameTime, _playerModel);

            // Handle Bullets
            foreach (var bullet in _bullets)
            {
                bullet.Update(gameTime);
            }

            // Remove Off-Screen Bullets
            _bullets.RemoveAll(b => b.IsOffScreen(_graphics.GraphicsDevice.Viewport));

            // Update Map Renderer
            _mapRenderer.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw Background
            _spriteBatch.Draw(_currentBackground, Vector2.Zero, Color.White);

            // Draw Map
            _mapRenderer.Draw();

            // Draw Player
            _playerView.Draw(_spriteBatch, _playerModel);

            // Draw Enemies
            _enemyLibrary.Draw(_spriteBatch);

            // Draw Bullets
            foreach (var bullet in _bullets)
            {
                bullet.Draw(_spriteBatch);
            }

            _spriteBatch.End();
        }

        public void FireBullet(Vector2 position, Vector2 direction)
        {
            var bullet = new Bullet(position, direction);
            _bullets.Add(bullet);
        }
    }
}
