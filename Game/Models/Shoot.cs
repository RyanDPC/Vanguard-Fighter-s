using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My2DGame
{
    public class Shoot
    {
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        private Texture2D texture;

        public Shoot(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            this.texture = texture;
            this.Position = position;
            this.Velocity = velocity;
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
