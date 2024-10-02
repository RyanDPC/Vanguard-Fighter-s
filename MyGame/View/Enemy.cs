using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MyGame.Models;

namespace MyGame.View
{
    public class Enemy
    {
        private MyGame.View.Enemy _view;
        private Texture2D _texture;
        private Texture2D Weapon;
        private Texture2D Bullet;
        private Vector2 Position;
        private const int EnemyWidth = 64;
        private const int EnemyHeight = 128;

        public Texture2D EnemyTexture { get; }

        public Enemy(MyGame.View.Enemy view, Vector2 initialPosition, Texture2D enemyTexture)
        {
            _view = view;
            Position = initialPosition;
            EnemyTexture = enemyTexture;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool isAlive)
        {
            if (isAlive)
            {
                // Crée un rectangle pour définir la position et la taille de l'ennemi à l'écran
                Rectangle destinationRectangle = new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    EnemyWidth,
                    EnemyHeight
                );

                // Dessine l'ennemi
                spriteBatch.Draw(_texture, destinationRectangle, Color.White);
            }
        }

        public void DrawWithHealthBar(SpriteBatch spriteBatch, Vector2 position, int health, int maxHealth)
        {
            // Dessiner l'ennemi
            Draw(spriteBatch, position, true);

            // Dessiner une barre de vie au-dessus de l'ennemi
            int healthBarWidth = EnemyWidth;
            int healthBarHeight = 150;
            float healthPercentage = (float)health / maxHealth;

            // Définir la couleur de la barre de vie en fonction des points de vie restants
            Color healthBarColor = healthPercentage > 0.5f ? Color.Green : (healthPercentage > 0.2f ? Color.Yellow : Color.Red);

            Rectangle healthBarBackground = new Rectangle((int)position.X, (int)position.Y - healthBarHeight - 2, healthBarWidth, healthBarHeight);
            Rectangle healthBarForeground = new Rectangle((int)position.X, (int)position.Y - healthBarHeight - 2, (int)(healthBarWidth * healthPercentage), healthBarHeight);

            // Dessiner l'arrière-plan de la barre de vie
            spriteBatch.Draw(_texture, healthBarBackground, Color.Gray);

            // Dessiner l'avant-plan (niveau de vie actuel)
            spriteBatch.Draw(_texture, healthBarForeground, healthBarColor);
        }
    }
}