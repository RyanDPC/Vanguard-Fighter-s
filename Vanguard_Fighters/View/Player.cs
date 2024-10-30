using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Models;
using MyGame.Services;
using System.Collections.Generic;

namespace MyGame.View
{
    public class Player
    {
        private List<Texture2D> _skins;
        private int _currentSkinIndex;
        private Vector2 _position;
        private WeaponView _weaponView;
        private Vector2 _weaponOffset;
        private float _scale;
        private bool isFacingRight;

        public List<Texture2D> Skins { get => _skins; set => _skins = value; }
        public int CurrentSkinIndex { get => _currentSkinIndex; set => _currentSkinIndex = value; }
        public Vector2 Position { get => _position; set => _position = value; }
        public WeaponView WeaponView { get => _weaponView; set => _weaponView = value; }
        public Vector2 WeaponOffset { get => _weaponOffset; set => _weaponOffset = value; }
        public float Scale { get => _scale; set => _scale = value; }

        public Player(List<Texture2D> skins, Weapon weapon, Vector2 initialPosition, bool isFacingRight, float scale, Vector2 weaponOffset)
        {
            Skins = skins;
            CurrentSkinIndex = 0;
            Position = initialPosition;
            Scale = scale;
            WeaponOffset = weaponOffset;
            WeaponView = new WeaponView(weapon, initialPosition, isFacingRight, Scale, WeaponOffset);
            this.isFacingRight = isFacingRight;
        }

        public void ChangeSkin(int change)
        {
            CurrentSkinIndex += change;
            if (CurrentSkinIndex >= Skins.Count) CurrentSkinIndex = 0;
            else if (CurrentSkinIndex < 0) CurrentSkinIndex = Skins.Count - 1;
        }

        // Met à jour la position et l'orientation en fonction de l'input
        public void Update(Vector2 newPosition, InputManager inputManager)
        {
            Position = newPosition;

            // Met à jour l'orientation selon l'input
            if (inputManager.GetMovement().X > 0)
                isFacingRight = true;
            else if (inputManager.GetMovement().X < 0)
                isFacingRight = false;

            WeaponView.Update(newPosition, isFacingRight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Utilise isFacingRight pour définir l'orientation
            SpriteEffects spriteEffect = !isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            // Dessine le skin du joueur avec l'orientation correcte
            spriteBatch.Draw(Skins[CurrentSkinIndex], Position, null, Color.White, 0f, Vector2.Zero, 0.130f, spriteEffect, 0f);

            // Dessine l'arme
            WeaponView.Draw(spriteBatch);
        }
    }
}
