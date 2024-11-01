using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Models;
using System.Collections.Generic;

namespace MyGame.View
{
    public class WeaponView
    {
        private Texture2D weaponTexture;
        private Texture2D bulletTexture;
        private List<Vector2> bulletsPositions;
        private bool isFacingRight;

        public WeaponView(Texture2D weaponTexture, Texture2D bulletTexture)
        {
            this.weaponTexture = weaponTexture;
            this.bulletTexture = bulletTexture;
            bulletsPositions = new List<Vector2>();
            isFacingRight = true;
        }

        // Méthode pour changer l'orientation de l'arme en fonction de la direction du joueur
        public void SetDirection(bool facingRight)
        {
            isFacingRight = facingRight;
        }

        // Ajoute une balle à la liste avec la position de départ
        public void AddBullet(Vector2 position)
        {
            bulletsPositions.Add(position);
        }

        // Met à jour la position des balles
        public void UpdateBullets(GameTime gameTime, float bulletSpeed, float scaleFactor)
        {
            for (int i = bulletsPositions.Count - 1; i >= 0; i--)
            {
                bulletsPositions[i] += new Vector2(bulletSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds * (isFacingRight ? 1 : -1) * scaleFactor, 0);

                // Retire les balles qui sortent de l'écran
                if (bulletsPositions[i].X > 1920 || bulletsPositions[i].X < 0)
                    bulletsPositions.RemoveAt(i);
            }
        }

        // Dessine l'arme du joueur ou de l'ennemi
        public void DrawWeapon(SpriteBatch spriteBatch, Vector2 position, float scaleFactor)
        {
            if (weaponTexture == null) return;

            SpriteEffects spriteEffect = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Rectangle destinationRectangle = new Rectangle(
                (int)(position.X * scaleFactor),
                (int)(position.Y * scaleFactor),
                (int)(weaponTexture.Width * scaleFactor),
                (int)(weaponTexture.Height * scaleFactor)
            );

            spriteBatch.Draw(weaponTexture, destinationRectangle, null, Color.White, 0f, Vector2.Zero, spriteEffect, 0f);
        }

        // Dessine les balles sur l'écran
        public void DrawBullets(SpriteBatch spriteBatch, float scaleFactor)
        {
            foreach (var bulletPos in bulletsPositions)
            {
                Rectangle bulletRectangle = new Rectangle(
                    (int)(bulletPos.X * scaleFactor),
                    (int)(bulletPos.Y * scaleFactor),
                    (int)(10 * scaleFactor), // Taille de la balle ajustée par le scaleFactor
                    (int)(10 * scaleFactor)
                );

                spriteBatch.Draw(bulletTexture, bulletRectangle, Color.White);
            }
        }
    }
}
