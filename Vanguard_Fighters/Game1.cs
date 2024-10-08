using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Models;
using MyGame.Services;
using MyGame.Library;
using System.Collections.Generic;
using Color = Microsoft.Xna.Framework.Color;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using System;

namespace Vanguard_Fighters
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;
        private TiledMapRenderer _mapRenderer;
        private Player player;
        private Enemy enemy;
        private InputManager inputManager;
        private List<TiledMap> maps; // Utiliser TiledMap de MonoGame.Extended
        private List<Texture2D> backgrounds;
        private Texture2D currentBackground;
        private TiledMap currentMap; // Utiliser TiledMap de MonoGame.Extended
        private Weapon weapon;
        private EnemyLibrary enemyLibrary;
        private int screenHeight;
        private int mapWidthInPixels;
        private int mapHeightInPixels;
        private int screenWidth;
        private bool isFacingRight;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            backgrounds = new List<Texture2D>();
            maps = new List<TiledMap>();

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            inputManager = new InputManager();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            WeaponLibrary weaponLibrary = new WeaponLibrary(Content);
            weaponLibrary.ContentWeapons();
            // Initialiser la bibliothèque d'armes
            weapon = weaponLibrary.GetWeapon("TacticalPistol");
            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;

            // Charger le background et la carte avec MonoGame.Extended
            backgrounds.Add(Content.Load<Texture2D>("Maps/Background"));
            maps.Add(Content.Load<TiledMap>("Textures/TheForest")); // Charger la carte avec MonoGame.Extended

            if (maps.Count > 0 && backgrounds.Count > 0 && maps[0] != null && backgrounds[0] != null)
            {
                // Assembler le premier background et la première map
                currentBackground = backgrounds[0];
                currentMap = maps[0];

                // Initialiser le renderer de la carte
                _mapRenderer = new TiledMapRenderer(GraphicsDevice, currentMap);
            }

            // Charger la texture du joueur
            Texture2D playerTexture = Content.Load<Texture2D>("Players/SpecialistFace");
            // Charger la texture de l'arme
            Texture2D weaponTexture = Content.Load<Texture2D>("Weapons/IonRifle");
            weapon.SetWeaponTexture(weaponTexture);
          

            // Obtenir les dimensions de la carte
            mapWidthInPixels = currentMap.WidthInPixels; // Utiliser WidthInPixels et HeightInPixels de MonoGame.Extended
            mapHeightInPixels = currentMap.HeightInPixels;

            // Initialiser le joueur
            Vector2 playerInitialPosition = new Vector2(
                (GraphicsDevice.PresentationParameters.BackBufferWidth - 64) / 2,
                GraphicsDevice.PresentationParameters.BackBufferHeight - 128
            );
            player = new Player(Content, playerTexture, playerInitialPosition, screenWidth, screenHeight, mapWidthInPixels, mapHeightInPixels, weapon);

            // Charger et initialiser l'ennemi
            Texture2D enemyTexture = Content.Load<Texture2D>("Players/WukongEntier");
            Vector2 enemyInitialPosition = new Vector2(500, 100);
            enemyLibrary = new EnemyLibrary(enemyTexture);
            enemyLibrary.AddEnemy(new Vector2(500, 100));
            enemyLibrary.AddEnemy(new Vector2(600, 150));

            enemy = new Enemy(enemyTexture, enemyInitialPosition);
        }

        private void LoadMap(int mapIndex)
        {
            if (mapIndex >= 0 && mapIndex < maps.Count && mapIndex < backgrounds.Count)
            {
                // Charger la carte actuelle
                currentMap = maps[mapIndex];
                _mapRenderer = new TiledMapRenderer(GraphicsDevice, currentMap);
                currentBackground = backgrounds[mapIndex];
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(mapIndex), "Invalid map.");
            }

            // Obtenir les dimensions de la carte
            mapWidthInPixels = currentMap.WidthInPixels;
            mapHeightInPixels = currentMap.HeightInPixels;
        }
        protected override void Update(GameTime gameTime)
        {
            inputManager.Update();

            if (inputManager.IsEscapePressed())
            {
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphics.ApplyChanges();
            }

            // Mettre à jour la carte
            _mapRenderer.Update(gameTime);

            // Mettre à jour le joueur
            player.Update(gameTime, inputManager, currentMap);

            // Mettre à jour l'ennemi
            enemy.MoveTowardsPlayer(player.Position, gameTime, GraphicsDevice, currentMap);

            // Gestion des collisions entre les projectiles et l'ennemi
            foreach (var bullet in player.Bullets)
            {
                if (bullet.Bounds.Intersects(enemy.GetEnemyRectangle()))
                {
                    enemy.TakeDamage(1); // L'ennemi perd 1 point de vie
                    if (!enemy.IsAlive) // Si l'ennemi n'est plus en vie
                    {
                        enemy.Die(); // L'ennemi meurt
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Dessiner le background
            if (currentBackground != null)
            {
                _spriteBatch.Draw(currentBackground, Vector2.Zero, Color.White);
            }

            _spriteBatch.End();

            // Dessiner la carte
            _mapRenderer.Draw();

            _spriteBatch.Begin();

            // Dessiner le joueur
            player.Draw(_spriteBatch, isFacingRight);

            // Dessiner l'ennemi
            enemyLibrary.DrawEnemies(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
