using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MyGame.Models;
using MyGame.Services;
using System.Collections.Generic;

namespace MyGame.Game
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TiledMapRenderer _mapRenderer;
        private Player player;
        private Enemy enemy;
        private InputManager inputManager;
        private List<TiledMap> maps; // Utiliser TiledMap de MonoGame.Extended
        private List<Texture2D> backgrounds;
        private Texture2D currentBackground;
        private TiledMap currentMap; // Utiliser TiledMap de MonoGame.Extended
        private Weapon weapon;
        private int screenHeight;
        private int mapWidthInPixels;
        private int mapHeightInPixels;
        private int screenWidth;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            backgrounds = new List<Texture2D>();
            maps = new List<TiledMap>(); // Modifier pour utiliser TiledMap de MonoGame.Extended

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            inputManager = new InputManager();
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;

            // Charger le background et la carte avec MonoGame.Extended
            backgrounds.Add(Content.Load<Texture2D>("Maps/Background"));
            maps.Add(Content.Load<TiledMap>("Textures/TheForest")); // Charger la carte avec MonoGame.Extended

            if (backgrounds.Count > 0 && maps.Count > 0)
            {
                // Assembler le premier background et la première map
                currentBackground = backgrounds[0];
                currentMap = maps[0];

                // Initialiser le renderer de la carte
                _mapRenderer = new TiledMapRenderer(GraphicsDevice, currentMap);
            }

            // Charger la texture du joueur
            Texture2D playerTexture = Content.Load<Texture2D>("Players/SpecialistFace");

            // Obtenir les dimensions de la carte
            mapWidthInPixels = currentMap.WidthInPixels;    // Utiliser WidthInPixels et HeightInPixels de MonoGame.Extended
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
            enemy = new Enemy(enemyTexture, enemyInitialPosition);
        }

        private void LoadMap(int mapIndex)
        {
            // Charger la carte actuelle
            currentMap = maps[mapIndex];
            _mapRenderer = new TiledMapRenderer(GraphicsDevice, currentMap);

            // Obtenir les dimensions de la carte
            mapWidthInPixels = currentMap.WidthInPixels;
            mapHeightInPixels = currentMap.HeightInPixels;

            // Charger le fond associé (si nécessaire)
            currentBackground = backgrounds[mapIndex];
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
            player.Draw(_spriteBatch);

            // Dessiner l'ennemi
            enemy.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
