using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Content.Weapons;

    


namespace MyGame.Models
{
    public class Bullet
    {
        public Rectangle Bounds { get; set; }
        public Vector2 Velocity {get; set;}
        private Texture2D _texture { get; set; }
        public Bullet(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            _texture = texture;
            Bounds = new Rectangle((int)position.X, (int)position.Y, 15, 7);
            Velocity = velocity;
        }
        public void Update(GameTime gameTime)
        {
            // Déplacer le projectile
            Bounds = new Rectangle(Bounds.X + (int)(Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds),
                               Bounds.Y + (int)(Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds),
                               Bounds.Width, Bounds.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Dessiner le mini-rectangle (projectile)
            spriteBatch.Draw(_texture, Bounds, Color.Red); // TextureManager.Pixel est une texture 1x1
        }
    }
}
