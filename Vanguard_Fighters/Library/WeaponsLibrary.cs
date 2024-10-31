using System;
using MyGame.Models;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;

namespace MyGame.Library
{ 
        public class WeaponsLibrary
        {
            private Dictionary<int, WeaponStats> weapons;

            public WeaponsLibrary(ContentManager contentManager)
            {

                //prendre le dictionnaire
                weapons = new Dictionary<int, WeaponStats>();

            //Ajout des armes
            weapons.Add(1, new WeaponStats("Advanced Assault Rifle",
                35, // Damage
                10f, // Fire rate (shots per second)
                30, // Max Ammo
                3, // Clip Size
                800f, // Range in pixels
                2.5f, // Reload Time (Example)
                "Automatic burst with slight accuracy loss", // Special ability
           contentManager.Load<Texture2D>("Weapons/Advanced_Assault_Rifle")));

            weapons.Add(2, new WeaponStats(
                "Compact Sidearm",
                25,
                5f,
                48,
                12,
                400f,
                1.2f,
                "Fast reload",
            contentManager.Load<Texture2D>("Weapons/Compact_Sidearm")));

            weapons.Add(3, new WeaponStats(
               "Energy Rifle",
               40,
               6f,
               80,
               20,
               1000f,
               3f,
               "Explosive shot every 5 shots",
            contentManager.Load<Texture2D>("Weapons/Energy_Rifle")));

            weapons.Add(4, new WeaponStats(
                "Futuristic Pistol",
                30,
                4f,
                40,
                10,
                500f,
                2f,
                "Overheats after 5 shots with increased damage",
            contentManager.Load<Texture2D>("Weapons/Futuristic_Pistol")));

            weapons.Add(5, new WeaponStats(
               "Ion Rifle",
               45,
               7f,
               100,
               25,
               900f,
               2.8f,
               "Temporarily disables enemy shields",
            contentManager.Load<Texture2D>("Weapons/Ion_Rifle")));

            weapons.Add(6, new WeaponStats(
                "Plasma Blaster",
                60,
                2f,
                32,
                8,
                600f,
                3.5f,
                "Delayed explosion causing area damage",
            contentManager.Load<Texture2D>("Weapons/Plasma_Blaster")));

            weapons.Add(7, new WeaponStats(
               "Sci-Fi Shotgun",
               80,
               1.5f,
               24,
               6,
               300f,
               3f,
               "Impulse that pushes nearby enemies",
            contentManager.Load<Texture2D>("Weapons/SciFi_Shotgun")));
            weapons.Add(8, new WeaponStats(
                "Stealth Handgun",
                20,
                3f,
                36,
                9,
                200f,
                2.2f,
                "Silent shot without revealing position",
            contentManager.Load<Texture2D>("Weapons/Stealth_Handgun")));

            weapons.Add(9, new WeaponStats(
               "Tactical Pistol",
               35,
               3f,
               40,
               10,
               600f,
               1.8f,
               "Precision mode with extended range",
            contentManager.Load<Texture2D>("Weapons/Tactical_Pistol")));
        }
        public WeaponStats GetWeapon(int index)
        {
            if (weapons.ContainsKey(index))
            {
                return weapons[index];
            }
            else
            {
                throw new KeyNotFoundException($"Weapon with index {index} does not exist.");
            }
        }
        public void SetWeaponTextures(int weaponIndex, Texture2D texture)
        {
            if (weapons.ContainsKey(weaponIndex))
            {
                WeaponStats weaponStats = weapons[weaponIndex];
                Weapon weapon = new Weapon(weaponStats, texture);
            }
        }
        public void TestWeapons(Texture2D bulletTexture)
        {
            foreach (var weaponStats in weapons.Values)
            {
                // Créer une instance de Weapon avec les stats et une texture fictive
                Weapon weapon = new Weapon(weaponStats, bulletTexture);

                // Tester les méthodes de l'arme
                weapon.Shoot(bulletTexture, new Vector2(0, 0), new Vector2(1, 0));
                weapon.Reload();
                weapon.UseSpecialAbility();
                // weapon.Reload(); -> Si tu implémentes le rechargement
                // weapon.UseSpecialAbility(); -> Si la capacité spéciale est implémentée
            }
        }

    }

}
