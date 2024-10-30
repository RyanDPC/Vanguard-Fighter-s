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

        public List<Texture2D> Skins { get => _skins; set => _skins = value; }
        public int CurrentSkinIndex { get => _currentSkinIndex; set => _currentSkinIndex = value; }
        public Vector2 Position { get => _position; set => _position = value; }
        public bool IsFacingRight { get => _isFacingRight; set => _isFacingRight = value; }
        public WeaponView WeaponView { get => _weaponView; set => _weaponView = value; }
        public Vector2 WeaponOffset { get => _weaponOffset; set => _weaponOffset = value; }
        public float Scale { get => _scale; set => _scale = value; }

        public Player(List<Texture2D> skins, Weapon weapon, Vector2 initialPosition, bool isFacingRight, float scale, Vector2 weaponOffset)
        {
            Skins = skins;
            CurrentSkinIndex = 0; // Commencer avec le premier skin
            Position = initialPosition;
            IsFacingRight = isFacingRight;
            Scale = scale;
            WeaponOffset = weaponOffset;
            WeaponView = new WeaponView(weapon, initialPosition, isFacingRight,Scale, WeaponOffset);
        }

        public void ChangeSkin(int change)
        {
            CurrentSkinIndex += change;
            if (CurrentSkinIndex >= Skins.Count) CurrentSkinIndex = 0;
            else if (CurrentSkinIndex < 0) CurrentSkinIndex = Skins.Count - 1;
        }

        public void Update(Vector2 newPosition, bool isFacingRight)
        {
            Position = newPosition;
            IsFacingRight = isFacingRight;
            WeaponView.Update(newPosition, isFacingRight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = !IsFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Skins[CurrentSkinIndex], Position, null, Color.White, 0f, Vector2.Zero, 0.130f, spriteEffect, 0f);
            WeaponView.Draw(spriteBatch);
        }
    }
}
