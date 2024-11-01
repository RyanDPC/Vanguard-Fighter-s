using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Vanguard_Fighters.Menu
{
    public class MainMenu
    {
        private Texture2D backgroundTexture;
        private Texture2D titleTexture;
        private Texture2D menuBackgroundTexture;
        private List<Button> buttons;

        private GraphicsDevice graphicsDevice;

        public MainMenu(Texture2D backgroundTexture, Texture2D titleTexture, Texture2D menuBackgroundTexture,
                        Texture2D playButtonTexture, Texture2D quitButtonTexture,
                        Texture2D optionButtonTexture, Texture2D editButtonTexture, GraphicsDevice graphicsDevice)
        {
            this.backgroundTexture = backgroundTexture;
            this.titleTexture = titleTexture;
            this.menuBackgroundTexture = menuBackgroundTexture;
            this.graphicsDevice = graphicsDevice;

            // Initialiser les boutons avec des ID
            buttons = new List<Button>
            {
                new Button(playButtonTexture, 1),  // Bouton "Play" avec ID 1
                new Button(quitButtonTexture, 2),  // Bouton "Quit" avec ID 2
                new Button(optionButtonTexture, 3), // Bouton "Options" avec ID 3 (non-fonctionnel)
                new Button(editButtonTexture, 4)    // Bouton "Edit" avec ID 4 (non-fonctionnel)
            };
        }

        public GameState Update(GameState currentState)
        {
            MouseState mouseState = Mouse.GetState();

            foreach (var button in buttons)
            {
                button.Update(mouseState);

                if (button.IsClicked(mouseState))
                {
                    switch (button.ButtonId)
                    {
                        case 1: // ID 1 : "Play"
                            return GameState.Playing;
                        case 2: // ID 2 : "Quit"
                            ExitGame();
                            break;
                        // Ajouter d'autres cas si nécessaire
                        default:
                            break;
                    }
                }
            }

            return currentState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Dessiner le fond du menu
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(menuBackgroundTexture, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.White * 0.5f);

            // Centrer et dessiner le titre
            Vector2 titlePosition = new Vector2(
                (graphicsDevice.Viewport.Width - titleTexture.Width) / 2,
                graphicsDevice.Viewport.Height * 0.1f
            );
            spriteBatch.Draw(titleTexture, titlePosition, Color.White);

            // Calculer la position initiale et espacement pour les boutons
            float buttonSpacing = 20f;
            Vector2 buttonStartPosition = new Vector2(
                (graphicsDevice.Viewport.Width - buttons[0].Texture.Width) / 2,
                titlePosition.Y + titleTexture.Height + buttonSpacing
            );

            // Afficher les boutons avec espacement dynamique
            for (int i = 0; i < buttons.Count; i++)
            {
                Vector2 buttonPosition = buttonStartPosition + new Vector2(0, i * (buttons[i].Texture.Height + buttonSpacing));
                buttons[i].Position = buttonPosition;
                buttons[i].Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        private void ExitGame()
        {
            System.Environment.Exit(0);
        }
    }
}
