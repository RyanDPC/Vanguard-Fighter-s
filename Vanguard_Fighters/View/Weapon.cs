
using Autofac;
using MyGame.Models;
using MyGame.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame.View
{
    public class WeaponView
    {
        public Texture2D WeaponTexture { get; private set; }

        private Weapon _weapon;
        private Vector2 _position;
        private bool _isFacingRight;
        private float _scale;
        private Vector2 _weaponOffset = new Vector2(0, 20);

        

        public WeaponView(Weapon weapon, Vector2 initialPostition, bool isFacingRight, float scale, Vector2 weaponOffset)
        {
            this._weapon = weapon;
            this._position = initialPostition;
            this._isFacingRight = isFacingRight;
            this._scale = scale ;
            this._weaponOffset = weaponOffset;
        }

        public void Update(Vector2 Position, bool isFacingRight)
        {
           this._position = Position + _weaponOffset;
           this._isFacingRight = isFacingRight;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = _isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(_weapon.WeaponTexture, _position, null, Color.White, 0f, Vector2.Zero,_scale, spriteEffect, 0f);
        }
    }
}
