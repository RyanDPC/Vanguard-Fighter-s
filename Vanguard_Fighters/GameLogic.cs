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
        private PlayerModel playerModel;
        private PlayerView playerView;
        private EnemyLibrary enemyLibrary;
        private Weapon weapon;
        private WeaponsLibrary weaponsLibrary;
        private InputManager inputManager;
        private Bullet bullet;
        private List<Bullet> bullets;
        private List<TiledMap> maps;
        private List<Texture2D> backgrounds;
        private Texture2D currentBackground;
        private TiledMap currentMap;
        private Vector2 weaponOffset = new Vector2(0, 40);
        private List<Texture2D> skins = new List<Texture2D>();
        private float scaleFactor; // Échelle appliquée à tous les éléments

        public GameLogic(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            _graphics = graphics;
            _spriteBatch = spriteBatch;
            inputManager = new InputManager();
            maps = new List<TiledMap>();
            backgrounds = new List<Texture2D>();
            bullets = new List<Bullet>();
        }

        public void LoadContent(ContentManager content)
        {
            Bullet.InitializeTexture(_graphics.GraphicsDevice);
            weaponsLibrary = new WeaponsLibrary(content, _graphics.GraphicsDevice);

            LoadMapsAndBackgrounds(content);
            LoadWeapon(9); // Exemple d'ID d'arme
            LoadPlayer(content);
            LoadEnemies(content);

            // Calcul de l'échelle de la carte en fonction de la taille de l'écran
            CalculateScaleFactor();
        }

        private void CalculateScaleFactor()
        {
            if (currentMap != null)
            {
                int mapWidth = currentMap.WidthInPixels;
                int mapHeight = currentMap.HeightInPixels;
                scaleFactor = Math.Min(
                    (float)_graphics.PreferredBackBufferWidth / mapWidth,
                    (float)_graphics.PreferredBackBufferHeight / mapHeight
                );
            }
        }

        private void LoadMapsAndBackgrounds(ContentManager content)
        {
            backgrounds.Add(content.Load<Texture2D>("Maps/Background"));
            maps.Add(content.Load<TiledMap>("Textures/TheForest"));

            if (maps.Count > 0 && backgrounds.Count > 0)
            {
                currentMap = maps[0];
                currentBackground = backgrounds[0];
                _mapRenderer = new TiledMapRenderer(_graphics.GraphicsDevice, currentMap);
            }
        }

        private void LoadWeapon(int weaponId)
        {
            WeaponStats weaponStat = weaponsLibrary.GetWeapon(weaponId);
            if (weaponStat != null)
            {
                weapon = new Weapon(weaponStat, weaponOffset * scaleFactor);
                inputManager.SetFireCooldown(weapon);
            }
            else
            {
                System.Console.WriteLine($"Erreur : Arme avec ID {weaponId} introuvable.");
            }
        }

        private void LoadPlayer(ContentManager content)
        {
            skins.Add(content.Load<Texture2D>("Players/SpecialistFace"));
            Vector2 playerInitialPosition = new Vector2(
                _graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2
            );

            playerModel = new PlayerModel(playerInitialPosition * scaleFactor, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, weapon, weaponsLibrary);
            playerView = new PlayerView(skins, weapon, playerInitialPosition, true, scaleFactor, weaponOffset * scaleFactor);
        }

        private void LoadEnemies(ContentManager content)
        {
            Texture2D enemyTexture = content.Load<Texture2D>("Players/WukongEntier");
            enemyLibrary = new EnemyLibrary(enemyTexture,bullet.);

            // Positionner les ennemis en tenant compte de l'échelle
            enemyLibrary.AddEnemy(new Vector2(500, 100) * scaleFactor);
            enemyLibrary.AddEnemy(new Vector2(600, 150) * scaleFactor);
        }

        public void Update(GameTime gameTime)
        {
            inputManager.Update(gameTime);

            if (inputManager.IsShootPressed())
            {
                playerModel.Shoot();
            }

            _mapRenderer.Update(gameTime);
            playerModel.Update(gameTime, inputManager, currentMap, scaleFactor);
            playerView.Update(playerModel.Position * scaleFactor, inputManager);
            enemyLibrary.UpdateEnemies(gameTime, playerModel.Position * scaleFactor, _graphics.GraphicsDevice, currentMap);
            HandleBulletCollisions();
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            // Dessiner l'arrière-plan
            if (currentBackground != null)
            {
                _spriteBatch.Draw(currentBackground, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            }

            foreach (var bullet in playerModel.GetBullets())
            {
                bullet.Draw(_spriteBatch, scaleFactor);
            }

            _spriteBatch.End();

            // Dessiner la carte avec l'échelle dynamique
            _mapRenderer.Draw(Matrix.CreateScale(scaleFactor));

            _spriteBatch.Begin();
            playerView.Draw(_spriteBatch,playerModel, scaleFactor);
            enemyLibrary.DrawEnemies(_spriteBatch, scaleFactor);
            _spriteBatch.End();
        }

        private void HandleBulletCollisions()
        {
            foreach (var bullet in playerModel.GetBullets())
            {
                foreach (var enemy in enemyLibrary.GetEnemies())
                {
                    if (bullet.Bounds.Intersects(enemy.GetEnemyRectangle()))
                    {
                        enemy.TakeDamage(bullet.Damage);
                        if (!enemy.IsAlive)
                        {
                            enemy.Die();
                        }
                    }
                }
            }
        }

        public bool IsPlayerDead() => playerModel.IsDead();
    }
}
