using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Services;

namespace MyGame.Models
{
    public class Bullet
    {
        public Rectangle Bounds { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public int Damage { get; private set; }
        private static Texture2D bulletTexture;
        private SpriteEffects spriteEffect;
        private float speed;
        private int width = 10; // Largeur du rectangle
        private int height = 5; // Hauteur du rectangle

        // Constructeur avec orientation et offset de l'arme
        public Bullet(Vector2 position, Vector2 velocity, int damage, bool isFacingRight, Vector2 weaponOffset, float speed)
        {
            Position = position + weaponOffset; // Ajoute l'offset de l'arme à la position initiale
            Velocity = velocity;
            Damage = damage;
            this.speed = speed;
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, width, height);
            spriteEffect = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }


        // Initialisation de la texture
        public static void InitializeTexture(GraphicsDevice graphicsDevice)
        {
            if (bulletTexture == null)
            {
                bulletTexture = new Texture2D(graphicsDevice, 1, 1);
                bulletTexture.SetData(new[] { Color.Red });
            }
        }

        public void Update(GameTime gameTime)
        {
            // Applique la vitesse multipliée par le temps écoulé
            Position += Velocity * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Mettre à jour les limites de la balle pour les collisions
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, width, height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Dessiner la balle avec l'orientation
            spriteBatch.Draw(bulletTexture, Position, null, Color.White, 0f, Vector2.Zero, 5f, spriteEffect, 0f);
        }

        public bool IsOffScreen(int screenWidth, int screenHeight)
        {
            return Position.X < 0 || Position.X > screenWidth || Position.Y < 0 || Position.Y > screenHeight;
        }
    }
}
