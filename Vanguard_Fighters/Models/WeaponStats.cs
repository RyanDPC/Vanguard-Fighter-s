using Microsoft.Xna.Framework.Graphics;

namespace MyGame.Models
{
    public class WeaponStats
    {
        public string Name { get; private set; }
        public int Damage { get; private set; }
        public float FireCooldown { get; private set; }
        public int MaxAmmo { get; set; }
        public float ReloadTime { get; private set; }
        public float Range { get; private set; }
        public int ClipSize { get; private set; }
        public string Ability {  get; private set; }
        public Texture2D WeaponTexture { get; private set; }
        public float Speed { get; private set; }

        public WeaponStats(string name, int damage, float fireCooldown, int maxAmmo, int clipSize,float range,float reloadTime, string ability, Texture2D texture, float speed)
        {
            Name = name;
            Damage = damage;
            FireCooldown = fireCooldown;
            MaxAmmo = maxAmmo;
            ClipSize = clipSize;
            Range = range;
            ReloadTime = reloadTime;
            Ability = ability;
            WeaponTexture = texture;
            Speed = speed;
        }
    }
}
