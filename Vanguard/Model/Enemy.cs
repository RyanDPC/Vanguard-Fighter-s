using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Vanguard.Service;
using Vanguard.View;

namespace Vanguard.Models
{
    public class Enemy
    {
        // Properties
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public int Health { get; private set; }
        public bool IsFacingRight { get; private set; }

        public Weapon EquippedWeapon { get; set; } // Equipped weapon

        // Fields
        private readonly WeaponView weaponView;
        private readonly List<Bullet> bullets;
        private readonly Player player;
        private readonly List<Platform> platforms;

        private const float Speed = 150f;        // Movement speed
        private const float JumpStrength = -400f; // Jump strength
        private const float Gravity = 1000f;     // Gravity force

        private float fireCooldownTimer = 10f;
        private float decisionCooldown;          // Cooldown for decision making
        private readonly int screenWidth;
        private readonly int screenHeight;

        private bool isOnGround;                 // Is the enemy on the ground
        private Texture2D healthBarTexture;      // Health bar texture

        private static Random random = new Random(); // Random number generator
        private float deltaTime;

        public event Action EnemyDied;

        // Constructor
        public Enemy(Vector2 startPosition, WeaponView weaponView, List<Bullet> bullets, Player player, List<Platform> platforms, int screenWidth, int screenHeight, GraphicsDevice graphicsDevice)
        {
            Position = startPosition;
            Velocity = Vector2.Zero;
            Health = 100;
            IsFacingRight = true;

            this.weaponView = weaponView;
            this.bullets = bullets;
            this.player = player;
            this.platforms = platforms;
            decisionCooldown = 1f;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.isOnGround = false;

            // Initialize the health bar texture
            healthBarTexture = new Texture2D(graphicsDevice, 1, 1);
            healthBarTexture.SetData(new[] { Color.White });
        }

        // Update method
        public void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            HandleLogic(gameTime);
            HandleShooting(gameTime);

            // Apply gravity to velocity
            Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * deltaTime);

            // Update position based on velocity
            Position += Velocity *  deltaTime;

            // Create local variables for position and velocity
            Vector2 position = Position;
            Vector2 velocity = Velocity;

            // Constrain position to screen bounds and adjust velocity if necessary
            Rectangle enemyRectangle = new Rectangle((int)Position.X, (int)Position.Y, 68, 128);
            Collision.ConstrainToScreen(enemyRectangle, screenWidth, screenHeight, ref position, ref velocity);

            // Update position and velocity
            Position = position;
            Velocity = velocity;

            // Check for ground collision
            CheckGroundCollision();

            // Check for collisions with platforms
            CheckPlatformCollisions(platforms);
        }

        // Handle AI logic for movement
        private void HandleLogic(GameTime gameTime)
        {
            decisionCooldown -= deltaTime;

            if (decisionCooldown <= 0)
            {
                decisionCooldown = 3.0f; // Take a new decision every second

                float distanceToPlayer = Vector2.Distance(Position, player.Position);

                if (distanceToPlayer > 200) // If too far, move closer
                {
                    Velocity = new Vector2(player.Position.X > Position.X ? Speed : -Speed, Velocity.Y);
                    IsFacingRight = player.Position.X > Position.X;
                }
                else if (distanceToPlayer < 150) // If too close, move away
                {
                    Velocity = new Vector2(player.Position.X < Position.X ? Speed : -Speed, Velocity.Y);
                    IsFacingRight = player.Position.X < Position.X;
                }
                else // Stay still
                {
                    Velocity = new Vector2(0, Velocity.Y);
                }

                // Randomly decide to jump
                if (isOnGround && random.Next(0, 2) == 0) // 50% chance to jump
                {
                    Velocity = new Vector2(Velocity.X, JumpStrength);
                    isOnGround = false;
                }
            }
        }

        // Handle shooting logic
        private void HandleShooting(GameTime gameTime)
        {
            fireCooldownTimer -= deltaTime;
            // Vérifier si une arme est équipée
            if (EquippedWeapon == null)
                return;

            // Si le chargeur est vide, recharger automatiquement
            if (EquippedWeapon.GetCurrentAmmo() <= 0)
            {
                EquippedWeapon.Reload();
                return;
            }

            // Vérifier si le joueur est à portée
            float distanceToPlayer = Vector2.Distance(Position, player.Position);
            if (distanceToPlayer > 300) // Si hors de portée, ne pas tirer
                return;

            // Déterminer la direction du tir
            Vector2 direction = IsFacingRight ? Vector2.UnitX : -Vector2.UnitX;

            // Taille du personnage (modifiable selon les dimensions de votre sprite)
            Vector2 characterSize = new Vector2(68, 128);

            // Position de l'arme (près de la main)
            Vector2 weaponPosition = Position + new Vector2(
                IsFacingRight ? characterSize.X * 0.7f : characterSize.X * 0.3f,
                characterSize.Y * 0.6f
            );

            // Position exacte pour générer une balle (bout de l'arme)
            Vector2 bulletSpawnPosition = weaponPosition + new Vector2(
                IsFacingRight ? characterSize.X / 2 : -characterSize.X / 2,
                -characterSize.Y / 4
            );

            // Vérifier la portée de tir
            if (distanceToPlayer <= EquippedWeapon.Range)
            {
                // Lancer le tir
                Bullet newBullet = new Bullet(bulletSpawnPosition, direction * EquippedWeapon.BulletSpeed, EquippedWeapon.Damage,EquippedWeapon.Range);
                bullets.Add(newBullet); // Ajouter la balle à la liste des balles

                if (EquippedWeapon.Shoot(gameTime, bulletSpawnPosition, direction, bullets))
                {
                    // Réinitialiser le cooldown après chaque tir
                    fireCooldownTimer = 3.0f; // Délai ajusté pour ralentir la cadence de tir
                }

            }

            // Mettre à jour l'arme (par exemple, gérer les temps de recharge)
            EquippedWeapon?.Update(gameTime);
        }

        

        // Check for collision with the ground
        private void CheckGroundCollision()
        {
            // Simple ground collision at the bottom of the screen
            if (Position.Y >= screenHeight - 128) // Assuming 128 is the enemy's height
            {
                Position = new Vector2(Position.X, screenHeight - 128);
                Velocity = new Vector2(Velocity.X, 0);
                isOnGround = true;
            }
            else
            {
                isOnGround = false;
            }
        }

        // Check for collisions with platforms
        private void CheckPlatformCollisions(List<Platform> platforms)
        {
            Rectangle enemyRect = new Rectangle((int)Position.X, (int)Position.Y, 68, 128);

            foreach (var platform in platforms)
            {
                if (enemyRect.Intersects(platform.Bounds))
                {
                    // Place the enemy on top of the platform
                    Position = new Vector2(Position.X, platform.Bounds.Top - 128);
                    Velocity = new Vector2(Velocity.X, 0);
                    isOnGround = true;
                    break;
                }
            }
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, 68, 128); // Taille de l'ennemi
        }

        // Apply damage to the enemy
        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Health = 0;
                // Add logic to remove the enemy or trigger a death animation
                EnemyDied?.Invoke();
            }
        }
    }
}
