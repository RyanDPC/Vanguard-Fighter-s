using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Vanguard.Menu
{
    public class GameMenu
    {
        // Champs et propriétés existants
        private Texture2D backgroundTexture;
        private Texture2D titleTexture;
        private Texture2D playButtonTexture;
        private Texture2D optionButtonTexture;
        private Texture2D editButtonTexture;
        private Texture2D quitButtonTexture;

        private Rectangle titleRect;
        private Rectangle playButtonRect;
        private Rectangle optionButtonRect;
        private Rectangle editButtonRect;
        private Rectangle quitButtonRect;

        private int screenWidth;
        private int screenHeight;

        // États de survol des boutons
        private bool isPlayHovered;
        private bool isOptionHovered;
        private bool isEditHovered;
        private bool isQuitHovered;

        // Événements pour les clics sur les boutons
        public event Action OnPlayClicked;
        public event Action OnOptionClicked;
        public event Action OnEditClicked;
        public event Action OnQuitClicked;

        // Constructeur
        public GameMenu(
            Texture2D backgroundTexture,
            Texture2D titleTexture,
            Texture2D playButtonTexture,
            Texture2D optionButtonTexture,
            Texture2D editButtonTexture,
            Texture2D quitButtonTexture,
            int screenWidth,
            int screenHeight)
        {
            this.backgroundTexture = backgroundTexture;
            this.titleTexture = titleTexture;
            this.playButtonTexture = playButtonTexture;
            this.optionButtonTexture = optionButtonTexture;
            this.editButtonTexture = editButtonTexture;
            this.quitButtonTexture = quitButtonTexture;
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

            // Calculer les dimensions des boutons et du titre
            int buttonWidth = (int)(screenWidth * 0.2f);  // 20% de la largeur de l'écran
            int buttonHeight = (int)(screenHeight * 0.1f); // 10% de la hauteur de l'écran
            int centerX = screenWidth / 2;

            // Redimensionner la texture du titre
            int titleWidth = (int)(screenWidth * 0.6f);
            int titleHeight = (int)(screenHeight * 0.2f);

            // Positionner le titre
            titleRect = new Rectangle(
                centerX - titleWidth / 2,
                (int)(screenHeight * 0.1f),
                titleWidth,
                titleHeight);

            // Espacement entre les boutons
            int spacing = (int)(screenHeight * 0.02f);

            // Point de départ pour le premier bouton
            int startY = titleRect.Bottom + spacing;

            // Positionner les boutons
            playButtonRect = new Rectangle(
                centerX - buttonWidth / 2,
                startY,
                buttonWidth,
                buttonHeight);

            optionButtonRect = new Rectangle(
                centerX - buttonWidth / 2,
                startY + buttonHeight + spacing,
                buttonWidth,
                buttonHeight);

            editButtonRect = new Rectangle(
                centerX - buttonWidth / 2,
                startY + 2 * (buttonHeight + spacing),
                buttonWidth,
                buttonHeight);

            quitButtonRect = new Rectangle(
                centerX - buttonWidth / 2,
                startY + 3 * (buttonHeight + spacing),
                buttonWidth,
                buttonHeight);
        }

        // Méthode Update
        public void Update(GameTime gameTime)
        {
            // Gérer les entrées utilisateur pour le menu
            MouseState mouseState = Mouse.GetState();
            Point mousePosition = new Point(mouseState.X, mouseState.Y);

            isPlayHovered = playButtonRect.Contains(mousePosition);
            isOptionHovered = optionButtonRect.Contains(mousePosition);
            isEditHovered = editButtonRect.Contains(mousePosition);
            isQuitHovered = quitButtonRect.Contains(mousePosition);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (isPlayHovered)
                {
                    OnPlayClicked?.Invoke();
                }
                else if (isOptionHovered)
                {
                    OnOptionClicked?.Invoke();
                }
                else if (isEditHovered)
                {
                    OnEditClicked?.Invoke();
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

            // Dessiner le titre
            spriteBatch.Draw(titleTexture, titleRect, Color.White);

            // Dessiner les boutons avec effet de surbrillance
            spriteBatch.Draw(playButtonTexture, playButtonRect, isPlayHovered ? Color.LightGray : Color.White);
            spriteBatch.Draw(optionButtonTexture, optionButtonRect, isOptionHovered ? Color.LightGray : Color.White);
            spriteBatch.Draw(editButtonTexture, editButtonRect, isEditHovered ? Color.LightGray : Color.White);
            spriteBatch.Draw(quitButtonTexture, quitButtonRect, isQuitHovered ? Color.LightGray : Color.White);
        }
    }
}
