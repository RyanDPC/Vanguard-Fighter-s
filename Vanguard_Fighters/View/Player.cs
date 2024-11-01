using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Models;
using MyGame.View;
using System;
using Vanguard_Fighters.Library;

namespace MyGame.View
{
    public class PlayerView
    {
        private Texture2D _playerTexture;
        private Weapon _weapon;
        private WeaponStats _weaponStats;
        private Vector2 _weaponOffset;
        private float _scaleFactor;

        public PlayerView(Texture2D playerTexture, Weapon weapon, Vector2 weaponOffset, float scaleFactor)
        {
            _playerTexture = playerTexture;
            _weapon = weapon;
            _weaponOffset = weaponOffset;
            _scaleFactor = scaleFactor;
        }

        public void Draw(SpriteBatch spriteBatch, PlayerModel playerModel)
        {
            Vector2 playerPosition = playerModel.Position;
            Rectangle playerDestinationRectangle = new Rectangle(
                (int)(playerPosition.X),
                (int)(playerPosition.Y),
                (int)(68 * _scaleFactor),  // Ajuster la largeur selon l'échelle
                (int)(128 * _scaleFactor)   // Ajuster la hauteur selon l'échelle
            );

            spriteBatch.Draw(_playerTexture, playerDestinationRectangle, Color.White);

            DrawWeapon(spriteBatch, playerModel);
        }
        private void DrawWeapon(SpriteBatch spriteBatch, PlayerModel playerModel)
        {
            Vector2 weaponPosition = playerModel.Position + _weaponOffset * _scaleFactor;
            var weaponDestinationRectangle = new Rectangle(
                (int)(weaponPosition.X),
                (int)(weaponPosition.Y),
                (int)(_weaponStats.WeaponTexture.Width * _scaleFactor),
                (int)(_weaponStats.WeaponTexture.Height * _scaleFactor)
            );

            SpriteEffects spriteEffects = playerModel.IsFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            // Ajoutez une variable rotation pour contrôler l'angle de l'arme
            float rotation = 0f; // Vous pouvez calculer cette valeur en fonction de la logique de votre jeu

            spriteBatch.Draw(
                _weaponStats.WeaponTexture,       // Texture de l'arme
                weaponPosition,                   // Position de l'arme
                null,                             // Source rectangle (null pour dessiner l'image entière)
                Color.White,                      // Couleur
                rotation,                         // Rotation de l'arme
                Vector2.Zero,                     // Point d'origine de la rotation
                _scaleFactor,                     // Échelle
                spriteEffects,                    // Effets pour flip
                0f                                // Profondeur
            );
        }

    }
}
