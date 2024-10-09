using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame.Models
{
    public class Bullet
    {
        public Rectangle Bounds { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        private Texture2D _texture;

        public Bullet(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            _texture = texture;
            Position = position;
            Velocity = velocity;
            Bounds = new Rectangle((int)position.X, (int)position.Y, 10, 5); // Taille de la balle
        }

        public void Update(GameTime gameTime)
        {
            // Calculer la nouvelle position en fonction de la vitesse
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Mettre à jour le Rectangle des collisions
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, Bounds.Width, Bounds.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Dessiner le projectile (mini-rectangle)
            spriteBatch.Draw(_texture, Bounds, Color.Red);
        }

        // Vérifier si la balle est hors de l'écran
        public bool IsOffScreen()
        {
            return (Position.X < 0 || Position.X > 1920 || Position.Y < 0 || Position.Y > 1080); // Adapter la taille de l'écran
        }
    }
}
