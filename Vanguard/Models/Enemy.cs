
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MyGameProjectComplete.Models
{
    public class Enemy
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; private set; }
        private Player player;  // Référence du joueur à poursuivre

        public Enemy(Texture2D texture, Vector2 initialPosition, Player targetPlayer)
        {
            Texture = texture;
            Position = initialPosition;
            player = targetPlayer;
        }
        public Rectangle GetBounds()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public void Update(GameTime gameTime)
        {
            // Logique du bot : suivre le joueur
            float speed = 2.0f;
            Vector2 direction = player.Position - Position;
            direction.Normalize();
            Position += direction * speed;

            // Esquiver les projectiles du joueur ou tirer avec précision
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
