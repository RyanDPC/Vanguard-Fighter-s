using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame.View
{
    public class EnemyView
    {
        private Texture2D _texture;

        public EnemyView(Texture2D texture)
        {
            _texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool isFacingRight, float scaleFactor)
        {
            SpriteEffects effects = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            // Applique le scaleFactor pour rendre l'affichage dynamique
            Rectangle destinationRectangle = new Rectangle(
                (int)(position.X * scaleFactor),
                (int)(position.Y * scaleFactor),
                (int)(64 * scaleFactor),   // Largeur de l'ennemi ajustée avec le scaleFactor
                (int)(128 * scaleFactor)   // Hauteur de l'ennemi ajustée avec le scaleFactor
            );

            spriteBatch.Draw(_texture, destinationRectangle, null, Color.White, 0f, Vector2.Zero, effects, 0f);
        }
    }
}
