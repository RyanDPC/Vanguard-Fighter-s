using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System.Collections.Generic;

namespace MyGame.Models
{
    public class EnemyLibrary
    {
        private List<Enemy> _enemies;
        private Texture2D _enemyTexture;
        private int currentEnemyIndex = 0; 

        public EnemyLibrary(Texture2D enemyTexture)
        {
            _enemies = new List<Enemy>();
            _enemyTexture = enemyTexture;
        }

        public void AddEnemy(Vector2 initialPosition)
        {
            Enemy newEnemy = new Enemy(_enemyTexture, initialPosition);
            _enemies.Add(newEnemy);
        }

        public void UpdateEnemies(GameTime gameTime, Vector2 playerPosition, GraphicsDevice graphicsDevice, TiledMap tiledMap)
        {
            if (_enemies.Count == 0) return;
            var currentEnemy = _enemies[currentEnemyIndex];
            if (currentEnemy.IsAlive)
            {
                currentEnemy.MoveTowardsPlayer(playerPosition, gameTime, graphicsDevice, tiledMap);
            }
            else
            {
                // Si l'ennemi est mort, passer à l'ennemi suivant
                currentEnemyIndex++;

                // Vérifier si on a atteint la fin de la liste des ennemis
                if (currentEnemyIndex >= _enemies.Count)
                {
                    currentEnemyIndex = _enemies.Count - 1; // Reste sur le dernier ennemi si tous sont morts
                }
            }
        }
        public void DrawEnemies(SpriteBatch spriteBatch)
        {
            if (_enemies.Count == 0) return; // Aucun ennemi à dessiner

            var currentEnemy = _enemies[currentEnemyIndex]; // Récupère l'ennemi actif

            if (currentEnemy.IsAlive)
            {
                currentEnemy.Draw(spriteBatch);
            }
        }

        public List<Enemy> GetEnemies()
        {
            return _enemies;
        }
    }
}
