using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGameProjectComplete.View
{
    public class Player
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; private set; }
        public Weapon Weapon { get; private set; }

        public Player(Texture2D texture, Vector2 initialPosition, Weapon weapon)
        {
            Weapon = weapon;
            Texture = texture;
            Position = initialPosition;
        }

        public void Update(GameTime gameTime)
        {
            // Logique de mise à jour minimaliste
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Dessiner le joueur
            spriteBatch.Draw(Texture, Position, Color.White);

            // Dessiner l'arme du joueur
            Weapon.Draw(spriteBatch); // Toujours face à droite ici
        }
    }
}
