using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vanguard.Models;

namespace Vanguard.View
{
    public class EnemyView
    {
        private Texture2D enemyTexture;
        private Texture2D healthBarTexture;
        private WeaponView weaponView;

        public EnemyView(Texture2D enemyTexture, WeaponView weaponView, GraphicsDevice graphicsDevice)
        {
            this.enemyTexture = enemyTexture;
            this.weaponView = weaponView;

            // Initialiser la texture de la barre de santé
            healthBarTexture = new Texture2D(graphicsDevice, 1, 1);
            healthBarTexture.SetData(new[] { Color.White });
        }

        public void Draw(SpriteBatch spriteBatch, Enemy enemy)
        {
            // Vérifier que l'ennemi n'est pas null
            if (enemy == null)
                return;

            SpriteEffects spriteEffect = enemy.IsFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            // Taille de l'ennemi
            Vector2 characterSize = new Vector2(68, 128);

            // Dessiner l'ennemi avec la taille spécifiée
            Rectangle destinationRectangle = new Rectangle(
                (int)enemy.Position.X,
                (int)enemy.Position.Y,
                (int)characterSize.X,
                (int)characterSize.Y
            );

            spriteBatch.Draw(enemyTexture, destinationRectangle, null, Color.White, 0f, Vector2.Zero, spriteEffect, 0.5f);

            // Position de l'arme près de la main de l'ennemi
            float weaponOffsetX = enemy.IsFacingRight ? characterSize.X * 0.1f : -characterSize.X * 0.1f;

            // Position de l'arme près de la main du personnage, avec décalage
            Vector2 weaponPosition = enemy.Position + new Vector2(
                !enemy.IsFacingRight ? characterSize.X * 0.7f + weaponOffsetX : characterSize.X * 0.3f + weaponOffsetX,
                characterSize.Y * 0.6f
            );

            // Dessiner l'arme équipée
            weaponView?.DrawWeapon(spriteBatch, weaponPosition, enemy.IsFacingRight, characterSize, 0.4f);

            // Dessiner la barre de santé
            DrawHealthBar(spriteBatch, enemy);
        }

        private void DrawHealthBar(SpriteBatch spriteBatch, Enemy enemy)
        {
            // Afficher les points de vie au-dessus de l'ennemi
            Vector2 healthBarPosition = new Vector2(enemy.Position.X, enemy.Position.Y - 10);
            int maxHealth = 100;
            int healthBarWidth = 50; // Ajustez si nécessaire
            int healthBarHeight = 5;

            // Calculer la largeur de la santé actuelle
            int currentHealthWidth = (int)((enemy.Health / (float)maxHealth) * healthBarWidth);

            // Dessiner l'arrière-plan (santé perdue)
            spriteBatch.Draw(healthBarTexture, new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, healthBarWidth, healthBarHeight), Color.Red);

            // Dessiner l'avant-plan (santé actuelle)
            spriteBatch.Draw(healthBarTexture, new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, currentHealthWidth, healthBarHeight), Color.Green);
        }
    }
}
