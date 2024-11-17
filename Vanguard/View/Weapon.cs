using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Vanguard.Models;

namespace Vanguard.View
{
    public class WeaponView
    {
        private List<Weapon> weapons;
        private int currentWeaponIndex;

        public WeaponView(List<Weapon> weaponList)
        {
            weapons = weaponList ?? new List<Weapon>();
            currentWeaponIndex = weapons.Count > 0 ? 0 : -1;
        }

        public Weapon GetCurrentWeapon()
        {
            if (weapons == null || weapons.Count == 0)
            {
                throw new InvalidOperationException("No weapons available in the list.");
            }

            if (currentWeaponIndex < 0 || currentWeaponIndex >= weapons.Count)
            {
                currentWeaponIndex = 0;
            }

            return weapons[currentWeaponIndex];
        }

        public void NextWeapon()
        {
            if (weapons == null || weapons.Count == 0) return;
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        }

        public void PreviousWeapon()
        {
            if (weapons == null || weapons.Count == 0) return;
            currentWeaponIndex = (currentWeaponIndex - 1 + weapons.Count) % weapons.Count;
        }

        public void DrawWeapon(SpriteBatch spriteBatch, Vector2 position, bool isFacingRight, Vector2 characterSize, float layerDepth)
        {
            Weapon currentWeapon = GetCurrentWeapon();
            if (currentWeapon != null && currentWeapon.Texture != null)
            {
                // Taille de l'arme : la moitié de la taille du personnage
                float weaponWidth = characterSize.X / 2;
                float weaponHeight = characterSize.Y / 2;

                // Calcul des facteurs d'échelle
                float scaleX = weaponWidth / currentWeapon.Texture.Width;
                float scaleY = weaponHeight / currentWeapon.Texture.Height;
                Vector2 scale = new Vector2(scaleX, scaleY);

                // Ajuster l'origine pour le retournement
                Vector2 origin = new Vector2(currentWeapon.Texture.Width / 2, currentWeapon.Texture.Height / 2);

                SpriteEffects spriteEffect = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                spriteBatch.Draw(
                    currentWeapon.Texture,
                    position,
                    null,
                    Color.White,
                    0f,
                    origin,
                    scale,
                    spriteEffect,
                    layerDepth
                );
            }
        }       
    }
}
