using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Vanguard.Menu
{
    public class GameOverMenu
    {
        // Champs et propriétés existants
        private Texture2D backgroundTexture;
        private SpriteFont font;
        private int screenWidth;
        private int screenHeight;

        private string gameOverText = "Game Over";
        private string playAgainText = "Play Again";
        private string quitText = "Quit";

        private Vector2 gameOverPosition;
        private Rectangle playAgainButtonRect;
        private Rectangle quitButtonRect;

        private bool isPlayAgainHovered;
        private bool isQuitHovered;

        // Événements pour les clics sur les boutons
        public event Action OnPlayAgainClicked;
        public event Action OnQuitClicked;

        // Constructeur
        public GameOverMenu(Texture2D backgroundTexture,SpriteFont font, int screenWidth, int screenHeight)
        {
            this.backgroundTexture = backgroundTexture;
            this.font = font;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            // Initialiser le layout du menu
            UpdateLayout(screenWidth, screenHeight);
        }

        // Méthode UpdateLayout
        public void UpdateLayout(int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            // Calculer les positions et les tailles des éléments
            Vector2 gameOverSize = font.MeasureString(gameOverText);
            Vector2 playAgainSize = font.MeasureString(playAgainText);
            Vector2 quitSize = font.MeasureString(quitText);

            int centerX = screenWidth / 2;

            // Position du texte "Game Over"
            gameOverPosition = new Vector2(centerX - gameOverSize.X / 2, screenHeight * 0.2f);

            // Bouton "Play Again"
            playAgainButtonRect = new Rectangle(
                centerX - (int)(playAgainSize.X / 2),
                (int)(screenHeight * 0.4f),
                (int)playAgainSize.X,
                (int)playAgainSize.Y);

            // Bouton "Quit"
            quitButtonRect = new Rectangle(
                centerX - (int)(quitSize.X / 2),
                (int)(screenHeight * 0.5f),
                (int)quitSize.X,
                (int)quitSize.Y);
        }

        // Méthode Update
        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Point mousePosition = new Point(mouseState.X, mouseState.Y);

            isPlayAgainHovered = playAgainButtonRect.Contains(mousePosition);
            isQuitHovered = quitButtonRect.Contains(mousePosition);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (isPlayAgainHovered)
                {
                    OnPlayAgainClicked?.Invoke();
                }
                else if (isQuitHovered)
                {
                    OnQuitClicked?.Invoke();
                }
            }
        }

        // Méthode Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // Dessiner l'arrière-plan
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

            // Dessiner le texte "Game Over"
            spriteBatch.DrawString(font, gameOverText, gameOverPosition, Color.White);

            // Dessiner les boutons avec effet de surbrillance
            Color playAgainColor = isPlayAgainHovered ? Color.LightGray : Color.White;
            Color quitColor = isQuitHovered ? Color.LightGray : Color.White;

            spriteBatch.DrawString(font, playAgainText, new Vector2(playAgainButtonRect.X, playAgainButtonRect.Y), playAgainColor);
            spriteBatch.DrawString(font, quitText, new Vector2(quitButtonRect.X, quitButtonRect.Y), quitColor);
        }
    }
}
