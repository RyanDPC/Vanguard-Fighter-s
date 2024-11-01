using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame.Models
{
    public class Bullet
    {
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public int Damage { get; private set; }
        private Texture2D _texture;
        private const float BulletSpeed = 1000f; // Vitesse de la balle
        private float _scaleFactor; // Facteur d'échelle pour adapter la taille

        public Bullet(Vector2 position, Vector2 direction, int damage, Texture2D texture, float scaleFactor)
        {
            Position = position;
            Velocity = direction * BulletSpeed; // Définir la vitesse de la balle
            Damage = damage;
            _texture = texture;
            _scaleFactor = scaleFactor;
        }

        public void Update(GameTime gameTime)
        {
            // Met à jour la position de la balle
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            // Dessine la balle avec un facteur d'échelle
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, _scaleFactor, SpriteEffects.None, 0f);
        }

        public bool IsOffScreen(int screenWidth, int screenHeight)
        {
            // Vérifie si la balle est en dehors des limites de l'écran
            return Position.X < 0 || Position.X > screenWidth || Position.Y < 0 || Position.Y > screenHeight;
        }
    }
}
