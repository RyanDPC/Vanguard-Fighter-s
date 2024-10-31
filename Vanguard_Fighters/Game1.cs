using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Models;
using MyGame.Services;
using MyGame.Library;
using MyGame.View;
using System.Collections.Generic;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled;
using System;

namespace Vanguard_Fighters
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TiledMapRenderer _mapRenderer;
        private MyGame.Models.Player playerModel;
        private MyGame.View.Player playerView;
        private Enemy enemy;
        private InputManager inputManager;
        private List<TiledMap> maps;
        private List<Texture2D> backgrounds;
        private Texture2D currentBackground;
        private TiledMap currentMap;
        private Weapon weapon;
        private WeaponView weapons;
        private WeaponsLibrary WeaponsLibrary;
        private EnemyLibrary enemyLibrary;
        private int screenWidth;
        private int screenHeight;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            backgrounds = new List<Texture2D>();
            maps = new List<TiledMap>();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            inputManager = new InputManager();
            WeaponsLibrary = new WeaponsLibrary(Content);

            // Charger les cartes et backgrounds
            LoadMapsAndBackgrounds();

            // Charger l'arme du joueur
            LoadWeapon(WeaponsLibrary);

            // Charger le joueur (model et view)
            LoadPlayer();

            // Charger et initialiser les ennemis
            LoadEnemies();
        }

        private void LoadMapsAndBackgrounds()
        {
            // Charger le background et la carte avec MonoGame.Extended
            backgrounds.Add(Content.Load<Texture2D>("Maps/Background"));
            maps.Add(Content.Load<TiledMap>("Textures/TheForest"));

            if (maps.Count > 0 && backgrounds.Count > 0)
            {
                currentMap = maps[0];
                currentBackground = backgrounds[0];
                _mapRenderer = new TiledMapRenderer(GraphicsDevice, currentMap);
            }
        }

        private void LoadWeapon(WeaponsLibrary weaponLibrary)
        {
            // Charger la texture de l'arme
            Texture2D weaponTexture = Content.Load<Texture2D>("Weapons/Ion_Rifle");
            // Initialiser l'arme
            WeaponStats weaponStat = weaponLibrary.GetWeapon(2); // Exemple: "Tactical Pistol"
            weapon = new MyGame.Models.Weapon(weaponStat, weaponTexture);
            Vector2 weaponOffset = new Vector2(0, 40);
            WeaponView weaponView = new WeaponView(weapon,new Vector2(100, 100), true, 0.5f, weaponOffset);
        }

        private void LoadPlayer()
        {
            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;

            // Charger la texture du joueur
            Texture2D playerTexture = Content.Load<Texture2D>("Players/SpecialistFace");
            if (playerTexture == null)
            {
                Console.WriteLine("Erreur : La texture du joueur n'a pas pu être chargée.");
            }

            // Initialiser le modèle et la vue du joueur
            Vector2 playerInitialPosition = new Vector2(
                (GraphicsDevice.PresentationParameters.BackBufferWidth - 64) / 2,
                GraphicsDevice.PresentationParameters.BackBufferHeight - 128
            );

            // Modèle du joueur (logique)
            playerModel = new MyGame.Models.Player(playerInitialPosition, screenWidth, screenHeight, weapon);

            // Vue du joueur (affichage)
            playerView = new MyGame.View.Player(playerTexture, weapon, playerInitialPosition, true);
        }

        private void LoadEnemies()
        {
            // Charger la texture de l'ennemi
            Texture2D enemyTexture = Content.Load<Texture2D>("Players/WukongEntier");

            // Initialiser la bibliothèque d'ennemis
            enemyLibrary = new EnemyLibrary(enemyTexture);
            enemyLibrary.AddEnemy(new Vector2(500, 100));
            enemyLibrary.AddEnemy(new Vector2(600, 150));

            // Initialiser un ennemi pour le test
            enemy = new Enemy(enemyTexture, new Vector2(500, 100));
        }

        protected override void Update(GameTime gameTime)
        {
            inputManager.Update();

            // Basculer en plein écran avec la touche Escape
            if (inputManager.IsEscapePressed())
            {
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphics.ApplyChanges();
            }

            // Mettre à jour la carte
            _mapRenderer.Update(gameTime);

            // Mettre à jour le modèle du joueur (logique)
            playerModel.Update(gameTime, inputManager, currentMap);

            // Mettre à jour la vue du joueur (position et direction)
            playerView.Update(playerModel.Position, playerModel.Velocity.X >= 0);

            // Mettre à jour les ennemis
            enemyLibrary.UpdateEnemies(gameTime, playerModel.Position, GraphicsDevice, currentMap);

            // Gestion des collisions entre les projectiles du joueur et l'ennemi
            HandleBulletCollisions();

            base.Update(gameTime);
        }

        private void HandleBulletCollisions()
        {
            foreach (var bullet in playerModel.Weapon.Bullets)
            {
                if (bullet.Bounds.Intersects(enemy.GetEnemyRectangle()))
                {
                    enemy.TakeDamage(1); // L'ennemi perd 1 point de vie

                    if (!enemy.IsAlive)
                    {
                        enemy.Die();
                    }
                }
            }
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

            // Dessiner la vue du joueur
            playerView.Draw(_spriteBatch);

            // Dessiner les ennemis
            enemyLibrary.DrawEnemies(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
