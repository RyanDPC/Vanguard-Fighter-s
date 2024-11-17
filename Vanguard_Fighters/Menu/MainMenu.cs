using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Vanguard_Fighters.Menu
{
    public class MainMenu
    {
        private Texture2D _backgroundTexture;
        private Texture2D _titleTexture;
        private Texture2D _playButtonTexture;
        private Texture2D _quitButtonTexture;

        private Rectangle _playButtonRectangle;
        private Rectangle _quitButtonRectangle;

        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        public MainMenu(Texture2D backgroundTexture, Texture2D titleTexture, Texture2D playButtonTexture, Texture2D quitButtonTexture, GraphicsDevice graphicsDevice)
        {
            _backgroundTexture = backgroundTexture;
            _titleTexture = titleTexture;
            _playButtonTexture = playButtonTexture;
            _quitButtonTexture = quitButtonTexture;

            // Positions et tailles des boutons
            _playButtonRectangle = new Rectangle(graphicsDevice.Viewport.Width / 2 - 100, 300, 200, 50);
            _quitButtonRectangle = new Rectangle(graphicsDevice.Viewport.Width / 2 - 100, 400, 200, 50);
        }

        public GameState Update(GameState currentGameState)
        {
            _currentMouseState = Mouse.GetState();

            // Vérifie si un clic a été effectué
            if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                Point mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                if (_playButtonRectangle.Contains(mousePosition))
                {
                    return GameState.Playing;
                }
                else if (_quitButtonRectangle.Contains(mousePosition))
                {
                    Game.Exit(); // Quitte le jeu
                }
            }

            _previousMouseState = _currentMouseState;
            return currentGameState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);
            spriteBatch.Draw(_titleTexture, new Vector2(200, 100), Color.White);
            spriteBatch.Draw(_playButtonTexture, _playButtonRectangle, Color.White);
            spriteBatch.Draw(_quitButtonTexture, _quitButtonRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
