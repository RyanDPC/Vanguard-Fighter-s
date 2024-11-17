using System;
using System.Collections.Generic;
using Vanguard.Models;

namespace Vanguard.Service
{
    public static class WeaponLibrary
    {
        public static Dictionary<string, Weapon> Weapons = new Dictionary<string, Weapon>();

        public static void InitializeWeapons()
        {
            Weapons = new Dictionary<string, Weapon>();

            // Example weapons with bullet speed and range
            Weapons["TacticalPistol"] = new Weapon("TacticalPistol", 10, 3.0f, 12, 1.5f, 0f, 10f, 50f, 300f, null);
            Console.WriteLine("Weapon initialized: TacticalPistol");
            Weapons["PlasmaBlaster"] = new Weapon("PlasmaBlaster", 25, 1.0f, 6, 2.0f, 0f, 0f, 40f, 600f, null);
            Console.WriteLine("Weapon initialized: PlasmaBlaster");
            Weapons["AdvancedAssaultRifle"] = new Weapon("AdvancedAssaultRifle", 15, 5.0f, 30, 2.5f, 0f, 0f, 60f, 100f, null);
            Console.WriteLine("Weapon initialized: AdvancedAssaultRifle");
            Weapons["CompactSidearm"] = new Weapon("CompactSidearm", 8, 4.0f, 15, 1.2f, 0f, 0f, 40f, 700f, null);
            Console.WriteLine("Weapon initialized: CompactSidearm");
            Weapons["EnergyRifle"] = new Weapon("EnergyRifle", 20, 2.0f, 10, 2.0f, 0f, 0f, 50f, 900f, null);
            Console.WriteLine("Weapon initialized: EnergyRifle");
            Weapons["SciFiShotgun"] = new Weapon("SciFiShotgun", 40, 0.8f, 5, 2.5f, 0f, 0f, 35f, 400f, null);
            Console.WriteLine("Weapon initialized: SciFiShotgun");
        }

        public static Weapon GetWeapon(string name)
        {
            if (Weapons.ContainsKey(name))
                return Weapons[name];
            else
                throw new KeyNotFoundException($"Weapon '{name}' not found in library.");
        }
    }
}