using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Models;
using SharpDX.Direct2D1.Effects;
using System.Collections.Generic;

namespace MyGame.View
{
    public class Player
    {
        private List<Texture2D> _skins; // Liste des skins
        private int _currentSkinIndex; // Index du skin actuel
        private Vector2 _position; // Position du joueur
        private bool _isFacingRight; // Direction du joueur
        private WeaponView _weaponView; // Vue pour l'arme du joueur
        private Vector2 _weaponOffset;
        private float _scale;
        public Player(List<Texture2D> skins, Weapon weapon, Vector2 initialPosition, bool isFacingRight,float scale, Vector2 weaponOffset)
        {
            _skins = skins;
            _currentSkinIndex = 0; // Commencer avec le premier skin
            _position = initialPosition;
            _isFacingRight = isFacingRight;
            _scale = scale;
            _weaponOffset = weaponOffset;
            _weaponView = new WeaponView(weapon, initialPosition, isFacingRight,_scale, _weaponOffset);
        }

        public void ChangeSkin(int change)
        {
            _currentSkinIndex += change;
            if (_currentSkinIndex >= _skins.Count) _currentSkinIndex = 0;
            else if (_currentSkinIndex < 0) _currentSkinIndex = _skins.Count - 1;
        }

        public void Update(Vector2 newPosition, bool isFacingRight)
        {
            _position = newPosition;
            _isFacingRight = isFacingRight;
            _weaponView.Update(newPosition, isFacingRight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = _isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(_skins[_currentSkinIndex], _position, null, Color.White, 0f, Vector2.Zero, 1.0f, spriteEffect, 0f);
            _weaponView.Draw(spriteBatch);
        }
    }
}
