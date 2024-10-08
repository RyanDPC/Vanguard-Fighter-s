using System;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Models;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;


namespace MyGame.Library
{
    public class WeaponLibrary
    {
        private Dictionary<string, Weapon> weapons;
        private ContentManager Content;
        public WeaponLibrary(ContentManager content)
        {
            this.Content = content;
            weapons = new Dictionary<string, Weapon>();
            ContentWeapons();
        }

        public void ContentWeapons()
        {
            weapons = new Dictionary<string, Weapon>();

            weapons.Add("AdvancedAssaultRifle", new Weapon(
               "Advanced Assault Rifle",
                35, // Damage
                10f, // Fire rate (shots per second)
                30, // Max Ammo
                3, // Clip Size
                800f, // Range in pixels
                2.5f, // Reload Time (Example)
                "Automatic burst with slight accuracy loss", // Special ability
               Content.Load<Texture2D>("Weapons/AdvancedAssaultRifle")
        ));
            weapons.Add("CompactSidearm", new Weapon(
                "Compact Sidearm",
                25,
                5f,
                48,
                12,
                400f,
                1.2f,
                "Fast reload",
                Content.Load<Texture2D>("Weapons/CompactSidearm")
        ));
            weapons.Add("EnergyRifle", new Weapon(
                "Energy Rifle",
                40,
                6f,
                80,
                20,
                1000f,
                3f,
                "Explosive shot every 5 shots",
                Content.Load<Texture2D>("Weapons/EnergyRifle")
        ));
            weapons.Add("FuturisticPistol", new Weapon(
                "Futuristic Pistol",
                30,
                4f,
                40,
                10,
                500f,
                2f,
                "Overheats after 5 shots with increased damage",
                Content.Load<Texture2D>("Weapons/FuturisticPistol")
        ));
            weapons.Add("IonRifle", new Weapon(
                "Ion Rifle",
                45,
                7f,
                100,
                25,
                900f,
                2.8f,
                "Temporarily disables enemy shields",
                Content.Load<Texture2D>("Weapons/IonRifle")
        ));
            weapons.Add("PlasmaBlaster", new Weapon(
                "Plasma Blaster",
                60,
                2f,
                32,
                8,
                600f,
                3.5f,
                "Delayed explosion causing area damage",
                Content.Load<Texture2D>("Weapons/PlasmaBlaster")
        ));
            weapons.Add("Sci-FiShotgun", new Weapon(
                "Sci-Fi Shotgun",
                80,
                1.5f,
                24,
                6,
                300f,
                3f,
                "Impulse that pushes nearby enemies",
                Content.Load<Texture2D>("Weapons/SciFiShotgun")
        ));
            weapons.Add("StealthHandgun", new Weapon(
                "Stealth Handgun",
                20,
                3f,
                36,
                9,
                200f,
                2.2f,
                "Silent shot without revealing position",
                Content.Load<Texture2D>("Weapons/StealthHandgun")
       ));
            weapons.Add("TacticalPistol", new Weapon(
                "Tactical Pistol",
                35,
                3f,
                40,
                10,
                600f,
                1.8f,
                "Precision mode with extended range",
                Content.Load<Texture2D>("Weapons/TacticalPistol")
       ));
        }
        public Weapon GetWeapon(string name)
        {
            if (weapons.TryGetValue(name, out Weapon weapon))
                return weapon;
            else
                throw new ArgumentException($"Weapon {name} not found in the library.");
        }
        public void SetWeaponTextures(Texture2D rifleTexture, Texture2D pistolTexture)
        {
            
            weapons["Advanced Assault Rifle"].SetWeaponTexture(rifleTexture);
            weapons["Compact Sidearm"].SetWeaponTexture(rifleTexture);
            weapons["Energy Rifle"].SetWeaponTexture(rifleTexture);
            weapons["Ion Rifle"].SetWeaponTexture(rifleTexture);
            weapons["Plasma Blaster"].SetWeaponTexture(rifleTexture);
            weapons["Sci-Fi Shotgun"].SetWeaponTexture(rifleTexture);
            //Pistol
            weapons["Stealth Handgun"].SetWeaponTexture(pistolTexture);
            weapons["Tactical Pistol"].SetWeaponTexture(pistolTexture);
            weapons["Futuristic Pistol"].SetWeaponTexture(pistolTexture);

        }
        public void TestWeapons()
        {
            foreach (var weapon in weapons.Values)
            {
                weapon.Shoot();
                weapon.Reload();
                weapon.UseSpecialAbility();
            }
        }

    }
}
