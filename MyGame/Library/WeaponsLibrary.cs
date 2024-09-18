using System;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Models;

namespace MyGame.Content.Weapons
{
    public class WeaponsLibrary
    {
        public Weapon AdvancedAssaultRifle { get; private set; }
        public Weapon CompactSidearm { get; private set; }
        public Weapon EnergyRifle { get; private set; }
        public Weapon FuturisticPistol { get; private set; }
        public Weapon IonRifle { get; private set; }
        public Weapon PlasmaBlaster { get; private set; }
        public Weapon SciFiShotgun { get; private set; }
        public Weapon StealthHandgun { get; private set; }
        public Weapon TacticalPistol { get; private set; }

        public WeaponsLibrary()
        {
            // Initialisation des armes avec leurs spécifications

            AdvancedAssaultRifle = new Weapon(
                "Advanced Assault Rifle",
                35,     // Dégâts
                10f,    // Cadence de tir (tirs par seconde)
                30,     // Capacité de munitions
                800f,   // Portée en pixels
                "Rafale automatique avec une légère perte de précision" // Capacité spéciale
            );

            CompactSidearm = new Weapon(
                "Compact Sidearm",
                25,
                5f,
                12,
                400f,
                "Recharge rapide"
            );

            EnergyRifle = new Weapon(
                "Energy Rifle",
                40,
                6f,
                20,
                1000f,
                "Tir explosif tous les 5 tirs"
            );

            FuturisticPistol = new Weapon(
                "Futuristic Pistol",
                30,
                4f,
                10,
                500f,
                "Surchauffe après 5 tirs avec dégâts accrus"
            );

            IonRifle = new Weapon(
                "Ion Rifle",
                45,
                7f,
                25,
                900f,
                "Désactive temporairement les boucliers ennemis"
            );

            PlasmaBlaster = new Weapon(
                "Plasma Blaster",
                60,
                2f,
                8,
                600f,
                "Explosion retardée infligeant des dégâts de zone"
            );

            SciFiShotgun = new Weapon(
                "Sci-Fi Shotgun",
                80,
                1.5f,
                6,
                300f,
                "Impulsion repoussant les ennemis proches"
            );

            StealthHandgun = new Weapon(
                "Stealth Handgun",
                20,
                3f,
                9,
                200f,
                "Tir silencieux sans révéler la position"
            );

            TacticalPistol = new Weapon(
                "Tactical Pistol",
                35,
                3f,
                10,
                600f,
                "Mode tir précis avec portée accrue"
            );
        }

        // Méthode pour tester les armes
        public void TestWeapons()
        {
            AdvancedAssaultRifle.Shoot();
            AdvancedAssaultRifle.Reload();
            AdvancedAssaultRifle.UseSpecialAbility();

            CompactSidearm.Shoot();
            CompactSidearm.Reload();
            CompactSidearm.UseSpecialAbility();

            // Testez les autres armes ici de manière similaire
        }

         public void SetWeaponTextures(Texture2D rifleTexture, Texture2D pistolTexture)
        {
            // Assigner les textures aux armes correspondantes
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
