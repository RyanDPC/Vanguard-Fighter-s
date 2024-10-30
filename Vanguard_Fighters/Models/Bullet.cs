using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame.Models
{
    public class Bullet
    {
        public Rectangle Bounds { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }

        public int Damage { get; private set; }
        private Texture2D _texture;
        private Texture2D circleTexture;

        public Bullet(Texture2D texture, Vector2 position, Vector2 velocity, int damage)
        {
            _texture = texture;
            Position = position;
            Velocity = velocity;
            Damage = damage;
            Bounds = new Rectangle((int)position.X, (int)position.Y, 50, 50); // Taille de la balle
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
        public bool IsOffScreen(int screenWidth, int screenHeight)
        {
            return (Position.X < 0 || Position.X > screenWidth || Position.Y < 0 || Position.Y > screenHeight); // Adapter la taille de l'écran
        }
    }
}
