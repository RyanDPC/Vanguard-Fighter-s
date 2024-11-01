using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Models;
using MyGame.View;
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
                (int)(_weaponStats.WeaponTexture.Width* _scaleFactor),
                (int)(_weaponStats.WeaponTexture.Height * _scaleFactor)
            );

            SpriteEffects spriteEffects = playerModel.IsFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(_weaponStats.WeaponTexture,weaponPosition,weaponDestinationRectangle, Color.White,roation ,0f, Vector2.Zero, spriteEffects, 0f);
        }
    }
}
