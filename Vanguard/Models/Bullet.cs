
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGameProjectComplete.Models
{
    public class Bullet
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }
        private Texture2D Texture;

        public Bullet(Texture2D texture, Vector2 position, Vector2 direction, float speed)
        {
            Texture = texture;
            Position = position;
            Direction = direction;
            Speed = speed;
        }

        public void Update(GameTime gameTime)
        {
            Position += Direction * Speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }
    }
}
