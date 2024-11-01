using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System;
using MyGame.Library;
using MyGame.View;
using Vanguard_Fighters.Library;

namespace MyGame.Models
{
    public class EnemyModel
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public int Health { get; set; } = 5;
        public bool IsAlive => Health > 0;
        public bool IsOnGround { get; private set; } = false;
        public bool IsFacingRight { get; private set; } = true;

        private const int EnemyWidth = 64;
        private const int EnemyHeight = 128;
        private float speed = 150.0f;
        private float gravity = 980f;
        private EnemyWeaponModel _weaponModel;
        private EnemyWeaponView _weaponView;

        public EnemyModel(Vector2 initialPosition, WeaponStats weaponStats, Texture2D bulletTexture)
        {
            Position = initialPosition;
            _weaponModel = new EnemyWeaponModel(weaponStats);
            _weaponView = new EnemyWeaponView(bulletTexture);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Console.WriteLine("Enemy is dead.");
            // Autres actions lors de la mort de l'ennemi, comme déclencher une animation de mort
        }

        public void MoveTowardsPlayer(Vector2 playerPosition, GameTime gameTime, TiledMap tiledMap, float scaleFactor)
        {
            Vector2 direction = playerPosition - Position;
            IsFacingRight = direction.X >= 0;

            if (Math.Abs(direction.X) > 0)
            {
                direction.Normalize();
                Position += new Vector2(direction.X * speed * (float)gameTime.ElapsedGameTime.TotalSeconds * scaleFactor, 0);
            }

            if (!IsOnGround)
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y + gravity * (float)gameTime.ElapsedGameTime.TotalSeconds * scaleFactor);
            }

            Position += new Vector2(0, Velocity.Y);
            HandleCollisions(tiledMap, scaleFactor);

            // Tirer seulement si aligné horizontalement avec le joueur
            if (Math.Abs(playerPosition.Y - Position.Y) < 10 && _weaponModel.CanShoot(gameTime))
            {
                _weaponModel.Shoot(gameTime);
                Vector2 bulletPosition = Position + new Vector2(IsFacingRight ? EnemyWidth : -10, EnemyHeight / 2);
                _weaponView.AddBullet(bulletPosition);
            }

            _weaponView.UpdateBullets(gameTime, _weaponModel.GetDamage(), scaleFactor);
        }

        private void HandleCollisions(TiledMap tiledMap, float scaleFactor)
        {
            IsOnGround = false;
            Rectangle enemyRect = new Rectangle((int)(Position.X * scaleFactor), (int)(Position.Y * scaleFactor), (int)(EnemyWidth * scaleFactor), (int)(EnemyHeight * scaleFactor));

            foreach (var layer in tiledMap.TileLayers)
            {
                foreach (var tile in layer.Tiles)
                {
                    if (tile.GlobalIdentifier > 0)
                    {
                        Rectangle tileRect = new Rectangle(
                            (int)(tile.X * tiledMap.TileWidth * scaleFactor),
                            (int)(tile.Y * tiledMap.TileHeight * scaleFactor),
                            (int)(tiledMap.TileWidth * scaleFactor),
                            (int)(tiledMap.TileHeight * scaleFactor)
                        );

                        if (enemyRect.Intersects(tileRect) && Velocity.Y > 0)
                        {
                            Position = new Vector2(Position.X, tileRect.Top / scaleFactor - EnemyHeight);
                            IsOnGround = true;
                            Velocity = new Vector2(Velocity.X, 0);
                        }
                    }
                }
            }
        }

        public Rectangle GetEnemyRectangle(float scaleFactor)
        {
            return new Rectangle((int)(Position.X * scaleFactor), (int)(Position.Y * scaleFactor), (int)(EnemyWidth * scaleFactor), (int)(EnemyHeight * scaleFactor));
        }

        public void Draw(SpriteBatch spriteBatch, float scaleFactor)
        {
            SpriteEffects effect = IsFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(
                _weaponView.BulletTexture,
                new Rectangle(
                    (int)(Position.X * scaleFactor),
                    (int)(Position.Y * scaleFactor),
                    (int)(EnemyWidth * scaleFactor),
                    (int)(EnemyHeight * scaleFactor)
                ),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                effect,
                0f
            );

            // Dessiner les balles tirées par l'ennemi
            _weaponView.Draw(spriteBatch, scaleFactor);
        }
    }
}
