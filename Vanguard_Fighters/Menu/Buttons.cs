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

        public Button(Texture2D texture, int buttonId)
        {
            Texture = texture;
            ButtonId = buttonId;
        }

        public bool IsClicked(MouseState currentMouseState)
        {
            var mouseRectangle = new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1);
            var buttonRectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            return mouseRectangle.Intersects(buttonRectangle) &&
                   currentMouseState.LeftButton == ButtonState.Pressed &&
                   previousMouseState.LeftButton == ButtonState.Released;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        public void Update(MouseState currentMouseState)
        {
            previousMouseState = currentMouseState;
        }
    }
}
