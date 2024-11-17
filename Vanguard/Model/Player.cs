using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Vanguard.Service;

namespace Vanguard.Models
{
    public class Player
    {
        // Propriétés
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public int Health { get; private set; }
        public bool IsFacingRight { get; private set; }

        public Weapon EquippedWeapon { get; set; } // Arme équipée

        // Champs
        private InputManager inputManager;
        private readonly List<Bullet> playerBullets;
        private readonly List<Platform> platforms; // Pour les collisions avec les plateformes

        private const float Speed = 200f;        // Vitesse de déplacement
        private const float JumpStrength = -400f; // Force du saut
        private const float Gravity = 1000f;     // Force de gravité

        private readonly int screenWidth;  // Largeur de l'écran
        private readonly int screenHeight; // Hauteur de l'écran

        private bool isOnGround;           // Le joueur est-il au sol ?

        // Événement pour la mort du joueur
        public event Action PlayerDied;

        // Constructeur
        public Player(Vector2 startPosition, InputManager inputManager, List<Bullet> bullets, List<Platform> platforms, int screenWidth, int screenHeight)
        {
            Position = startPosition;
            Velocity = Vector2.Zero;
            Health = 100;
            IsFacingRight = true;

            this.inputManager = inputManager;
            this.playerBullets = bullets;
            this.platforms = platforms;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.isOnGround = false;
        }

        // Méthode Update
        public void Update(GameTime gameTime)
        {
            HandleMovement(gameTime);
            HandleShooting(gameTime);
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Appliquer la gravité à la vélocité
            Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * deltaTime);

            // Mettre à jour la position en fonction de la vélocité
            Position += Velocity * deltaTime;

            // Réinitialiser isOnGround
            isOnGround = false;

            // Vérifier les collisions avec les plateformes
            CheckPlatformCollisions(platforms);

            // Vérifier la collision avec le sol
            CheckGroundCollision();

            // Gérer le rechargement
            HandleReloading();

            // Mettre à jour l'arme équipée
            EquippedWeapon?.Update(gameTime);

            // Create local variables for position and velocity
            Vector2 position = Position;
            Vector2 velocity = Velocity;

            // Contraindre la position aux limites de l'écran et ajuster la vélocité si nécessaire
            Rectangle playerRectangle = new Rectangle((int)Position.X, (int)Position.Y, 68, 128);
            Collision.ConstrainToScreen(playerRectangle, screenWidth, screenHeight, ref position, ref velocity);

            // Mettre à jour la position et la vélocité
            Position = position;
            Velocity = velocity;
        }

        // Gérer le mouvement du joueur
        private void HandleMovement(GameTime gameTime)
        {
            Vector2 movement = Vector2.Zero;

            if (inputManager != null)
            {
                movement = inputManager.GetMovement();

                // Gérer le saut
                if (inputManager.IsJumpPressed() && isOnGround)
                {
                    Velocity = new Vector2(Velocity.X, JumpStrength);
                    isOnGround = false;
                }
            }

            if (movement != Vector2.Zero)
            {
                // Normaliser et appliquer la vitesse à la vélocité
                movement.Normalize();
                Velocity = new Vector2(movement.X * Speed, Velocity.Y);
                IsFacingRight = movement.X > 0;
            }
            else
            {
                // Appliquer la friction lorsqu'aucune entrée n'est détectée
                float friction = 0.9f; // Ajustez le coefficient de friction si nécessaire
                Velocity = new Vector2(Velocity.X * friction, Velocity.Y);

                // Arrêter le mouvement horizontal si la vélocité est très faible
                if (Math.Abs(Velocity.X) < 0.1f)
                {
                    Velocity = new Vector2(0, Velocity.Y);
                }
            }
        }

        // Gérer le tir du joueur
        private void HandleShooting(GameTime gameTime)
        {
            if (inputManager.IsShootPressed() && EquippedWeapon != null)
            {
                Console.WriteLine("SHooting");
                Vector2 direction = IsFacingRight ? Vector2.UnitX : -Vector2.UnitX;

                // Position de l'arme près de la main
                Vector2 weaponPosition = Position + new Vector2(
                    IsFacingRight ? 68 * 0.7f : 68 * 0.3f,
                    128 * 0.6f
                );

                bool fired = EquippedWeapon.Shoot(gameTime, weaponPosition, direction, playerBullets);
                if (fired)
                    Console.WriteLine($"Player fired a bullet at {weaponPosition} in direction {direction}");
            }
            EquippedWeapon?.Update(gameTime);
        }

        private void HandleReloading()
        {
            if (inputManager.IsReloadPressed() && EquippedWeapon != null)
            {
                EquippedWeapon.Reload();
            }
        }

        // Vérifier la collision avec le sol
        private void CheckGroundCollision()
        {
            // Vérifier les collisions avec le sol ou les plateformes
            foreach (var platform in platforms)
            {
                if (Position.Y + 128 >= platform.Bounds.Top && Position.X + 68 > platform.Bounds.Left && Position.X < platform.Bounds.Right)
                {
                    Position = new Vector2(Position.X, platform.Bounds.Top - 128);
                    Velocity = new Vector2(Velocity.X, 0);
                    isOnGround = true;
                    return;
                }
            }

            // Collision simple avec le bas de l'écran
            if (Position.Y >= screenHeight - 128)
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

        // Vérifier les collisions avec les plateformes
        private void CheckPlatformCollisions(List<Platform> platforms)
        {
            Rectangle playerRect = new Rectangle((int)Position.X, (int)Position.Y, 68, 128);

            foreach (var platform in platforms)
            {
                // Vérifier si le joueur entre en collision avec le dessous de la plateforme
                Rectangle bottomCollisionRect = new Rectangle(platform.Bounds.Left, platform.Bounds.Top - 10, platform.Bounds.Width, 10);
                if (playerRect.Intersects(bottomCollisionRect) && Velocity.Y < 0)
                {
                    // Si le joueur saute et frappe le dessous de la plateforme, arrêter le saut
                    Velocity = new Vector2(Velocity.X, 0);
                    return;
                }

                if (playerRect.Intersects(platform.Bounds) && Velocity.Y >= 0 && Position.Y + 128 <= platform.Bounds.Top + 5)
                {
                    // Place the player on top of the platform only if falling down and not jumping through
                    Position = new Vector2(Position.X, platform.Bounds.Top - 128);
                    Velocity = new Vector2(Velocity.X, 0);
                    isOnGround = true;
                    break;
                }
            }
        }

        // Obtenir la hitbox du joueur
        public Rectangle GetHitbox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, 68, 128); // Taille du joueur
        }

        // Appliquer des dégâts au joueur
        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Health = 0;
                // Invoquer l'événement de mort du joueur
                PlayerDied?.Invoke();
            }
        }
    }
}