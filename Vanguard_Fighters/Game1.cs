using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vanguard_Fighters.Menu;

namespace Vanguard_Fighters
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameState currentGameState;
        private GameLogic gameLogic;
        private MainMenu mainMenu;
        private GameOverMenu gameOverMenu;

        private Texture2D backgroundTexture;
        private Texture2D titleTexture;
        private Texture2D menuBackgroundTexture;
        private Texture2D buttonTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            currentGameState = GameState.MainMenu;

            // Configure screen size
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Initialisation du fond blanc pour le menu principal
            backgroundTexture = new Texture2D(GraphicsDevice, 1, 1);
            backgroundTexture.SetData(new[] { Color.White });

            // Chargement des textures pour le titre, le fond du menu, et les boutons spécifiques avec le bon chemin
            Texture2D titleTexture = Content.Load<Texture2D>("Menu/Title");           // Chemin : Content/Menu/Title.png
            Texture2D menuBackgroundTexture = Content.Load<Texture2D>("Menu/BackGroundMenu"); // Chemin : Content/Menu/BackGroundMenu.png

            // Charger les textures pour chaque bouton (Play, Quit, Option, Edit)
            Texture2D playButtonTexture = Content.Load<Texture2D>("Menu/Play");       // Chemin : Content/Menu/Play.png
            Texture2D quitButtonTexture = Content.Load<Texture2D>("Menu/Quit");       // Chemin : Content/Menu/Quit.png
            Texture2D optionButtonTexture = Content.Load<Texture2D>("Menu/Option");   // Chemin : Content/Menu/Option.png
            Texture2D editButtonTexture = Content.Load<Texture2D>("Menu/Edit");       // Chemin : Content/Menu/Edit.png

            // Initialisation du menu principal avec les textures de fond et de boutons nécessaires
            mainMenu = new MainMenu(menuBackgroundTexture,titleTexture,menuBackgroundTexture, playButtonTexture, quitButtonTexture, optionButtonTexture, editButtonTexture, GraphicsDevice);

            // Initialisation du menu de Game Over avec les textures de fond et de boutons
            gameOverMenu = new GameOverMenu(backgroundTexture,titleTexture, menuBackgroundTexture, playButtonTexture, quitButtonTexture,optionButtonTexture,editButtonTexture, GraphicsDevice);

            // Initialisation de GameLogic avec les éléments nécessaires
            gameLogic = new GameLogic(_graphics, _spriteBatch);
            gameLogic.LoadContent(Content);
        }


        protected override void Update(GameTime gameTime)
        {
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    currentGameState = mainMenu.Update(currentGameState);

                    // Permet de passer à l'état Playing en appuyant sur "P"
                    if (Keyboard.GetState().IsKeyDown(Keys.P))
                    {
                        currentGameState = GameState.Playing;
                    }
                    break;

                case GameState.Playing:
                    gameLogic.Update(gameTime);
                    if (gameLogic.IsPlayerDead())
                    {
                        currentGameState = GameState.GameOver;
                    }
                    break;

                case GameState.GameOver:
                    currentGameState = gameOverMenu.Update(currentGameState);
                    break;
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    mainMenu.Draw(_spriteBatch);
                    break;

                case GameState.Playing:
                    gameLogic.Draw(gameTime);
                    break;

                case GameState.GameOver:
                    gameOverMenu.Draw(_spriteBatch);
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
