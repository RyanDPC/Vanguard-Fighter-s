
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGameProjectComplete.View
{
    public class Map
    {
        public Texture2D BackgroundTexture { get; set; }

        public Map(Texture2D backgroundTexture)
        {
            BackgroundTexture = backgroundTexture;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(BackgroundTexture, position, Color.White);
        }
    }
}
