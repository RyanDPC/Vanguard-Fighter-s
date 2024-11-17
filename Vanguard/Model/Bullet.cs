using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Vanguard.Models
{
    public class Bullet
    {
        // Propriétés existantes
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public bool IsActive { get; set; }
        public int Damage { get; set; }
        public float Range { get; set; }
        public float TraveledDistance { get; set; }

        // Ajouter des champs pour la largeur et la hauteur de la balle
        private const int Width = 10;  // Ajustez selon vos besoins
        private const int Height = 4;  // Ajustez selon vos besoin

        // Ajouter la propriété Bounds
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    Width,
                    Height
                );
            }
        }

        // Constructeur
        public Bullet(Vector2 position, Vector2 velocity, int damage, float range)
        {
            Position = position;
            Velocity = velocity;
            Damage = damage;
            Range = range;
            IsActive = true;
            TraveledDistance = 0f;
        }

        // Méthode Update
        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 movement = Velocity * deltaTime;
            Position += movement;
            TraveledDistance += movement.Length();

            Console.WriteLine($"Bullet updated: Position={Position}, TraveledDistance={TraveledDistance}, Velocity={Velocity}");

            // Désactiver la balle si elle a parcouru sa portée maximale
            if (TraveledDistance >= Range)
            {
                IsActive = false;
                Console.WriteLine("Bullet deactivated: Out of range");
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D debugTexture, Color color)
        {
            if (IsActive)
            {
                Rectangle bulletRect = new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    10, // Largeur de la balle
                    4   // Hauteur de la balle
                );

                spriteBatch.Draw(debugTexture, bulletRect, color);
            }
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, 10, 4); // Ajustez la taille si nécessaire
        }
    }
}
