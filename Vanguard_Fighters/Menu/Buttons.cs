using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Vanguard_Fighters.Menu
{
    public class Button
    {
        public Texture2D Texture { get; private set; }
        public int ButtonId { get; private set; } // ID unique pour le bouton
        public Vector2 Position { get; set; }

        private MouseState previousMouseState;
        private bool isHovered;

        public Button(Texture2D texture, int buttonId)
        {
            Texture = texture;
            ButtonId = buttonId;
        }

        public bool IsClicked(MouseState currentMouseState)
        {
            var mouseRectangle = new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1);
            var buttonRectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            // Détecte si la souris clique sur le bouton
            return mouseRectangle.Intersects(buttonRectangle) &&
                   currentMouseState.LeftButton == ButtonState.Pressed &&
                   previousMouseState.LeftButton == ButtonState.Released;
        }

        public void Update(MouseState currentMouseState)
        {
            // Met à jour l'état précédent de la souris
            var buttonRectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            isHovered = buttonRectangle.Contains(currentMouseState.Position);

            previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Change la couleur si le bouton est survolé
            Color buttonColor = isHovered ? Color.Gray : Color.White;
            spriteBatch.Draw(Texture, Position, buttonColor);
        }
    }
}
