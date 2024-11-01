using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MyGame.Models;
using MyGame.View;
using System;
using System.Collections.Generic;

namespace MyGame.Library
{
    public class EnemyLibrary
    {
        private List<EnemyModel> _enemies;
        private EnemyView _enemyView;
        private Vector2 weaponOffset = new Vector2(0, 40); // Offset pour l'arme de l'ennemi

        public EnemyLibrary(Texture2D enemyTexture, Texture2D bulletTexture)
        {
            _enemies = new List<EnemyModel>();
            _enemyView = new EnemyView(enemyTexture);
        }

        public void AddEnemy(Vector2 position)
        {
            // Initialiser chaque ennemi avec un Tactical Pistol (ID: 9)
            var tacticalPistol = new Weapon(9, weaponOffset);
            _enemies.Add(new EnemyModel(position, tacticalPistol));
        }

        public void UpdateEnemies(GameTime gameTime, Vector2 playerPosition, TiledMap tiledMap, float scaleFactor)
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.IsAlive)
                {
                    enemy.MoveTowardsPlayer(playerPosition, gameTime, tiledMap);

                    // Vérifie si l'ennemi est sur la même ligne horizontale que le joueur pour tirer
                    if (Math.Abs(enemy.Position.Y - playerPosition.Y) < 50) // Ajuster la tolérance selon besoin
                    {
                        enemy.Shoot(playerPosition, gameTime); // L'ennemi tire s'il est en ligne avec le joueur
                    }
                }
            }
        }

        public void DrawEnemies(SpriteBatch spriteBatch, float scaleFactor)
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.IsAlive)
                {
                    _enemyView.Draw(spriteBatch, enemy.Position, enemy.IsFacingRight, scaleFactor);
                    _enemyView.DrawWeapon(spriteBatch, enemy.Position, enemy.IsFacingRight, scaleFactor);
                }
            }
        }

        public List<EnemyModel> GetEnemies()
        {
            return _enemies;
        }
    }
}
