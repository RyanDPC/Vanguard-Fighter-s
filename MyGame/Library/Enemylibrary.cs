using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Content.Weapons;
using MyGame.Models;
using System;
using System.Collections.Generic;

namespace MyGame.Library
{
    public class EnemyLibrary
    {
        private List<Enemy> enemies;
        private Random random;
        private Texture2D enemyTexture;
        private MyGame.Content.Weapons.WeaponLibrary weaponLibrary;

        public EnemyLibrary(Texture2D enemyTexture, WeaponLibrary weaponLibrary)
        {
            enemies = new List<Enemy>();
            random = new Random();
            this.enemyTexture = enemyTexture;
            this.weaponLibrary = weaponLibrary;
        }

        public void InitializeEnemies(int numberOfEnemies)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                AddEnemy();
            }
        }

        public void AddEnemy()
        {
            // Position aléatoire sur la carte
            int xPosition = random.Next(100, 1800);
            int yPosition = random.Next(100, 900);
            WeaponLibrary weaponLibrary = new WeaponLibrary();

            List<Weapon> weapons = new List<Weapon>()
            {
                weaponLibrary.IonRifle,
                weaponLibrary.EnergyRifle,
                weaponLibrary.AdvancedAssaultRifle,
                weaponLibrary.CompactSidearm,
                weaponLibrary.StealthHandgun,
                weaponLibrary.PlasmaBlaster,
                weaponLibrary.TacticalPistol,
                weaponLibrary.SciFiShotgun,
                weaponLibrary.FuturisticPistol,
            };
            int randomIndex = random.Next(weapons.Count);
            // Sélectionner une arme aléatoire
            Weapon assignedWeapon = weapons[randomIndex] ;

            // Créer un nouvel ennemi avec une position aléatoire et une arme
            Enemy newEnemy = new Enemy(enemyTexture, new Vector2(xPosition, yPosition), assignedWeapon);
            enemies.Add(newEnemy);
        }

        public void RemoveEnemy(Enemy enemy)
        {
            enemies.Remove(enemy);
            AddEnemy(); // Ajoute un autre ennemi après la mort de celui-ci
        }

        public void Update(GameTime gameTime, Player player)
        {
            foreach (var enemy in enemies)
            {
                enemy.Update();

                // Vérifie si l'ennemi est mort
                if (enemy.IsDead())
                {
                    RemoveEnemy(enemy);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }
    }
}
