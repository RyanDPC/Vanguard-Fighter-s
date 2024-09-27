
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGameProjectComplete.View
{
    public class Weapon
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; private set; }

        public Weapon(Texture2D texture, Vector2 initialPosition)
        {
            Texture = texture;
            Position = initialPosition;
        }

        public void Update(GameTime gameTime)
        {
            // Logique de tir
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
