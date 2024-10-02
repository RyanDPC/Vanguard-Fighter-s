using System;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Models;

namespace MyGame.Content.Weapons
{
    public class WeaponLibrary
    {
        public Weapon AdvancedAssaultRifle { get;  set; }
        public Weapon CompactSidearm { get;  set; }
        public Weapon EnergyRifle { get; private set; }
        public Weapon FuturisticPistol { get; private set; }
        public Weapon IonRifle { get; private set; }
        public Weapon PlasmaBlaster { get; private set; }
        public Weapon SciFiShotgun { get; private set; }
        public Weapon StealthHandgun { get; private set; }
        public Weapon TacticalPistol { get; private set; }

        public WeaponLibrary()
        {
            // Initialization of weapons with their specifications

            AdvancedAssaultRifle = new Weapon(
                "Advanced Assault Rifle",
                35,     // Damage
                10f,    // Fire rate (shots per second)
                30,     // Ammo capacity
                800f,   // Range in pixels
                "Automatic burst with slight accuracy loss" // Special ability
            );

            CompactSidearm = new Weapon(
                "Compact Sidearm",
                25,
                5f,
                12,
                400f,
                "Fast reload"
            );

            EnergyRifle = new Weapon(
                "Energy Rifle",
                40,
                6f,
                20,
                1000f,
                "Explosive shot every 5 shots"
            );

            FuturisticPistol = new Weapon(
                "Futuristic Pistol",
                30,
                4f,
                10,
                500f,
                "Overheats after 5 shots with increased damage"
            );

            IonRifle = new Weapon(
                "Ion Rifle",
                45,
                7f,
                25,
                900f,
                "Temporarily disables enemy shields"
            );

            PlasmaBlaster = new Weapon(
                "Plasma Blaster",
                60,
                2f,
                8,
                600f,
                "Delayed explosion causing area damage"
            );

            SciFiShotgun = new Weapon(
                "Sci-Fi Shotgun",
                80,
                1.5f,
                6,
                300f,
                "Impulse that pushes nearby enemies"
            );

            StealthHandgun = new Weapon(
                "Stealth Handgun",
                20,
                3f,
                9,
                200f,
                "Silent shot without revealing position"
            );

            TacticalPistol = new Weapon(
                "Tactical Pistol",
                35,
                3f,
                10,
                600f,
                "Precision mode with extended range"
            );
        }
        //public void GetWeapon() 
        //{

        //}
        // Method to test the weapons
        public void TestWeapons()
        {
            AdvancedAssaultRifle.Shoot();
            AdvancedAssaultRifle.Reload();
            AdvancedAssaultRifle.UseSpecialAbility();

            CompactSidearm.Shoot();
            CompactSidearm.Reload();
            CompactSidearm.UseSpecialAbility();

            // Test other weapons similarly
        }

        // Method to assign textures to weapons
        public void SetWeaponTextures(Texture2D rifleTexture, Texture2D pistolTexture)
        {
            // Assign textures to corresponding weapons
            AdvancedAssaultRifle.SetWeaponTexture(rifleTexture);
            IonRifle.SetWeaponTexture(rifleTexture);
            PlasmaBlaster.SetWeaponTexture(rifleTexture);
            SciFiShotgun.SetWeaponTexture(rifleTexture);
            EnergyRifle.SetWeaponTexture(rifleTexture);

            CompactSidearm.SetWeaponTexture(pistolTexture);
            FuturisticPistol.SetWeaponTexture(pistolTexture);
            StealthHandgun.SetWeaponTexture(pistolTexture);
            TacticalPistol.SetWeaponTexture(pistolTexture);
        }
    }
}
