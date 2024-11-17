using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vanguard.Models;

namespace Vanguard.View
{
    public class PlayerView
    {
        private Texture2D playerTexture;
        private Texture2D healthBarTexture;
        private WeaponView weaponView;

        public PlayerView(Texture2D playerTexture, WeaponView weaponView, GraphicsDevice graphicsDevice)
        {
            this.playerTexture = playerTexture;
            this.weaponView = weaponView;

            // Initialiser la texture de la barre de santé
            healthBarTexture = new Texture2D(graphicsDevice, 1, 1);
            healthBarTexture.SetData(new[] { Color.White });
        }

        // Méthode pour dessiner le joueur
        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            SpriteEffects spriteEffect = !player.IsFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            // Taille du personnage
            Vector2 characterSize = new Vector2(68, 128);

            // Dessiner le sprite du joueur
            Rectangle destinationRectangle = new Rectangle(
                (int)player.Position.X,
                (int)player.Position.Y,
                (int)characterSize.X,
                (int)characterSize.Y
            );

            spriteBatch.Draw(playerTexture, destinationRectangle, null, Color.White, 0f, Vector2.Zero, spriteEffect, 0f);

            // Position de l'arme près de la main du joueur
            Vector2 weaponPosition = player.Position + new Vector2(
                player.IsFacingRight ? characterSize.X * 0.7f : characterSize.X * 0.3f,
                characterSize.Y * 0.6f
            );

            // Dessiner l'arme équipée
            weaponView?.DrawWeapon(spriteBatch, weaponPosition, player.IsFacingRight, characterSize, 0.4f);

            // Dessiner la barre de santé
            DrawHealthBar(spriteBatch, player);
        }

        // Méthode pour dessiner la barre de santé du joueur
        private void DrawHealthBar(SpriteBatch spriteBatch, Player player)
        {
            Vector2 healthBarPosition = new Vector2(player.Position.X, player.Position.Y - 10);
            int maxHealth = 100;
            int healthBarWidth = 50; // Ajustez si nécessaire
            int healthBarHeight = 5;

            // Calculer la largeur de la santé actuelle
            int currentHealthWidth = (int)((player.Health / (float)maxHealth) * healthBarWidth);

            // Dessiner l'arrière-plan (santé perdue)
            spriteBatch.Draw(healthBarTexture, new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, healthBarWidth, healthBarHeight), Color.Red);

            // Dessiner l'avant-plan (santé actuelle)
            spriteBatch.Draw(healthBarTexture, new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, currentHealthWidth, healthBarHeight), Color.Green);
        }
    }
}
