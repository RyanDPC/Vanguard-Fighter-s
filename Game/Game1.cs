using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace My2DGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

       

        private Texture2D projectileTexture;
        private List<Shoot> shoots;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            shoots = new List<Shoot>();
        }

        protected override void Initialize()
        {
            // Initialisation ici
            playerPosition = new Vector2(100, 100);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Créer la texture du joueur (un carré rouge de 50x50 pixels)
            playerTexture = new Texture2D(GraphicsDevice, 50, 50);
            Color[] playerData = new Color[50 * 50];
            for (int i = 0; i < playerData.Length; ++i) playerData[i] = Color.Red;
            playerTexture.SetData(playerData);

            // Créer la texture du tir (un petit rectangle blanc de 10x5 pixels)
            projectileTexture = new Texture2D(GraphicsDevice, 10, 5);
            Color[] projectileData = new Color[10 * 5];
            for (int i = 0; i < projectileData.Length; ++i) projectileData[i] = Color.White;
            projectileTexture.SetData(projectileData);
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            // Déplacement du joueur
            if (keyboardState.IsKeyDown(Keys.A))
                playerPosition.X -= 5f;
            if (keyboardState.IsKeyDown(Keys.D))
                playerPosition.X += 5f;
            if (keyboardState.IsKeyDown(Keys.W))
                playerPosition.Y -= 5f;
            if (keyboardState.IsKeyDown(Keys.S))
                playerPosition.Y += 5f;

            // Gérer le tir avec le clic gauche
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

                // Calcul de la direction du tir
                Vector2 direction = mousePosition - playerPosition;
                direction.Normalize(); // On normalise pour garder une vitesse constante

                // Vitesse du tir
                Vector2 velocity = direction * 300f; // Ajustez la vitesse du tir ici

                // Créer le tir
                shoots.Add(new Shoot(projectileTexture, playerPosition, velocity));
            }

            // Mise à jour des tirs
            for (int i = shoots.Count - 1; i >= 0; i--)
            {
                shoots[i].Update(gameTime);

                // Si le tir est hors de l'écran, on le retire
                if (shoots[i].Position.X < 0 || shoots[i].Position.X > _graphics.PreferredBackBufferWidth ||
                    shoots[i].Position.Y < 0 || shoots[i].Position.Y > _graphics.PreferredBackBufferHeight)
                {
                    shoots.RemoveAt(i);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Dessiner le joueur
            _spriteBatch.Draw(playerTexture, playerPosition, Color.White);

            // Dessiner les tirs
            foreach (var shoot in shoots)
            {
                shoot.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
