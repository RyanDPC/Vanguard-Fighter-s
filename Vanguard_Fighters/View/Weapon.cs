
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
        public Weapon Weapon { get => _weapon; set => _weapon = value; }
        public Vector2 Position { get => _position; set => _position = value; }
        public bool IsFacingRight { get => _isFacingRight; set => _isFacingRight = value; }
        public float Scale { get => _scale; set => _scale = value; }
        public Vector2 WeaponOffset { get => _weaponOffset; set => _weaponOffset = value; }

        private Weapon _weapon;
        private Vector2 _position;
        private bool _isFacingRight;
        private float _scale;
        private Vector2 _weaponOffset = new Vector2(0, 20);

        

        public WeaponView(Weapon weapon, Vector2 initialPostition, bool isFacingRight, float scale, Vector2 weaponOffset)
        {
            this.Weapon = weapon;
            this.Position = initialPostition;
            this.IsFacingRight = isFacingRight;
            this.Scale = scale ;
            this.WeaponOffset = weaponOffset;
        }

        public void Update(Vector2 Position, bool isFacingRight)
        {
           this.Position = Position + WeaponOffset;
           this.IsFacingRight = isFacingRight;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = IsFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Weapon.Texture, Position, null, Color.White, 0f, Vector2.Zero,0.130f, spriteEffect, 0f);
        }
    }
}
