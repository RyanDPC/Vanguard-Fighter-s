using Microsoft.Xna.Framework;
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
        private int screenWidth;

        private Map _map;
        private Player player;
        private Enemy enemy;
        private InputManager inputManager;
        private int screenHeight;
        private int mapWidthInPixels;
        private int mapHeightInPixels;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            inputManager = new InputManager();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

        screenWidth = _graphics.PreferredBackBufferWidth;
        screenHeight = _graphics.PreferredBackBufferHeight;
             // Charger les cartes Tiled et leurs fonds associés
            List<TiledMap> maps = new List<TiledMap>
            {
                Content.Load<TiledMap>("Textures/TheForest")
            };

            // Obtenir les dimensions de la carte
            TiledMap currentMap = maps[0];
            mapWidthInPixels = currentMap.Width * currentMap.TileWidth;    // Largeur de la carte en pixels
            mapHeightInPixels = currentMap.Height * currentMap.TileHeight; // Hauteur de la carte en pixels

            List<Texture2D> backgrounds = new List<Texture2D>
            {
                Content.Load<Texture2D>("Maps/Background")  // Associer le fond avec la carte Tiled
            };

            // Initialiser la carte avec le fond associé
            _map = new Map(GraphicsDevice, maps, backgrounds);

            // Charger la texture du joueur
            Texture2D playerTexture = Content.Load<Texture2D>("Players/SpecialistFace");

            // Initialiser le joueur
            Vector2 playerInitialPosition = new Vector2(
                (GraphicsDevice.PresentationParameters.BackBufferWidth - 64) / 2,
                GraphicsDevice.PresentationParameters.BackBufferHeight - 128
            );
            player = new Player(playerTexture, playerInitialPosition, screenWidth, screenHeight, mapWidthInPixels, mapHeightInPixels);

            // Charger et initialiser l'ennemi
            Texture2D enemyTexture = Content.Load<Texture2D>("Players/WukongEntier");
            Vector2 enemyInitialPosition = new Vector2(500, 100);
            enemy = new Enemy(enemyTexture, enemyInitialPosition);
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
            _map.Update(gameTime);

            // Mettre à jour le joueur
            player.Update(gameTime, inputManager, _map.GetCurrentTiledMap());

            // Mettre à jour l'ennemi
            enemy.MoveTowardsPlayer(player.Position, gameTime, GraphicsDevice, _map.GetCurrentTiledMap());

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Dessiner la carte et le fond
            _map.Draw(_spriteBatch);

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
