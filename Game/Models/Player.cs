using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Game.Content.Models
{
    internal class Player
    {
        private Texture2D playerTexture;
        public Vector2 playerPosition;
        public Rectangle hitbox;

        public Player(Texture2D texture, Vector2 initialPosition)
        {
            playerTexture = texture;
            playerPosition = initialPosition;
            UpdateHitbox();
        }

        // Call this function whenever the player moves to update the hitbox
        private void UpdateHitbox()
        {
            hitbox = new Rectangle(
                (int)playerPosition.X,
                (int)playerPosition.Y,
                playerTexture.Width,
                playerTexture.Height
            );
        }

        public void Move(Vector2 newPosition)
        {
            playerPosition = newPosition;
            UpdateHitbox();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, playerPosition, Color.White);
        }
    }
}
