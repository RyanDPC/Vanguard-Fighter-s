using Microsoft.Xna.Framework;
using MyGame.Models;
using Vanguard_Fighters.Library;

namespace MyGame.Library
{
    public class EnemyWeaponModel
    {
        private WeaponStats _weaponStats;
        private float _fireCooldown;
        private float _lastShotTime;
        private bool _isReloading;

        public EnemyWeaponModel(WeaponStats weaponStats)
        {
            _weaponStats = weaponStats;
            _fireCooldown = weaponStats.FireRate;
            _lastShotTime = 0f;
            _isReloading = false;
        }

        public bool CanShoot(GameTime gameTime)
        {
            return (float)gameTime.TotalGameTime.TotalSeconds - _lastShotTime >= _fireCooldown && !_isReloading;
        }

        public void Shoot(GameTime gameTime)
        {
            _lastShotTime = (float)gameTime.TotalGameTime.TotalSeconds;
        }

        public void StartReloading()
        {
            _isReloading = true;
        }

        public void FinishReloading()
        {
            _isReloading = false;
        }

        public int GetDamage()
        {
            return _weaponStats.Damage;
        }
    }
}
