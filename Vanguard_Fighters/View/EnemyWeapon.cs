using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MyGame.View
{
    public class EnemyWeaponView
    {
        public Texture2D BulletTexture { get; private set; }
        private List<Vector2> _bullets; // Stocke les positions des balles

        public EnemyWeaponView(Texture2D bulletTexture)
        {
            BulletTexture = bulletTexture;
            _bullets = new List<Vector2>();
        }

        public void AddBullet(Vector2 position)
        {
            _bullets.Add(position);
        }

        public void UpdateBullets(GameTime gameTime, float bulletSpeed, float scaleFactor)
        {
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                _bullets[i] += new Vector2(bulletSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds * scaleFactor, 0);

                // Retire les balles qui sortent de l'écran
                if (_bullets[i].X > 1920 || _bullets[i].X < 0) // Utilisez la largeur de l'écran dynamiquement si possible
                    _bullets.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch spriteBatch, float scaleFactor)
        {
            foreach (var bullet in _bullets)
            {
                spriteBatch.Draw(BulletTexture, new Rectangle((int)(bullet.X * scaleFactor), (int)(bullet.Y * scaleFactor), (int)(10 * scaleFactor), (int)(10 * scaleFactor)), Color.White);
            }
        }
    }
}
