using Microsoft.Xna.Framework.Graphics;

namespace Vanguard_Fighters.Library
{
    // Classe contenant les statistiques et caractéristiques de chaque arme.
    public class WeaponStats
    {
        // Propriétés de l'arme
        public string Name { get; private set; }           // Nom de l'arme
        public int Damage { get; private set; }            // Dégâts de l'arme
        public float FireRate { get; private set; }        // Cadence de tir (délai entre les tirs)
        public int MaxAmmo { get; set; }                   // Munitions maximales disponibles pour l'arme
        public float ReloadTime { get; private set; }      // Temps de rechargement en secondes
        public float Range { get; private set; }           // Portée effective de l'arme
        public int ClipSize { get; private set; }          // Capacité du chargeur
        public string Ability { get; private set; }        // Capacité spéciale de l'arme
        public Texture2D WeaponTexture { get; private set; } // Texture de l'arme
        public float Speed { get; private set; }           // Vitesse du projectile

        // Constructeur pour initialiser tous les attributs
        public WeaponStats(string name, int damage, float fireCooldown, int maxAmmo, int clipSize, float range, float reloadTime, string ability, Texture2D texture, float speed)
        {
            Name = name;
            Damage = damage;
            FireRate = fireCooldown;
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
