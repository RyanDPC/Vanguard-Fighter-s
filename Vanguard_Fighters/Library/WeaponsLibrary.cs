using System;
using MyGame.Models;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Vanguard_Fighters.Library;

namespace MyGame.Library
{
    public class WeaponsLibrary
    {
        private Dictionary<int, WeaponStats> weapons;
        private Texture2D bulletTexture;
        private GraphicsDevice graphicsDevice;
        private ContentManager contentManager;

        public WeaponsLibrary(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this.contentManager = contentManager ?? throw new ArgumentNullException(nameof(contentManager), "ContentManager cannot be null.");
            this.graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice), "GraphicsDevice cannot be null.");

            weapons = new Dictionary<int, WeaponStats>();
            LoadWeapons();
        }

        private void LoadWeapons()
        {
            weapons = new Dictionary<int, WeaponStats>();

            // Ajout des armes avec des textures chargées dynamiquement
            weapons.Add(1, new WeaponStats(
                name: "Advanced Assault Rifle",
                damage: 15,
                fireCooldown: 0.4f,
                maxAmmo: 30,
                clipSize: 3,
                range: 800f,
                reloadTime: 2.5f,
                ability: "Automatic burst with slight accuracy loss",
                texture: contentManager.Load<Texture2D>("Weapons/Advanced_Assault_Rifle"),
                speed: 15f));

            weapons.Add(2, new WeaponStats(
                name: "Compact Sidearm",
                damage: 10,
                fireCooldown: 0.8f,
                maxAmmo: 48,
                clipSize: 12,
                range: 400f,
                reloadTime: 1.2f,
                ability: "Fast reload",
                texture: contentManager.Load<Texture2D>("Weapons/Compact_Sidearm"),
                speed: 15f));

            weapons.Add(3, new WeaponStats(
                name: "Energy Rifle",
                damage: 18,
                fireCooldown: 0.6f,
                maxAmmo: 80,
                clipSize: 20,
                range: 1000f,
                reloadTime: 3f,
                ability: "Explosive shot every 5 shots",
                texture: contentManager.Load<Texture2D>("Weapons/Energy_Rifle"),
                speed: 15f));

            weapons.Add(4, new WeaponStats(
                name: "Futuristic Pistol",
                damage: 12,
                fireCooldown: 1.5f,
                maxAmmo: 40,
                clipSize: 10,
                range: 500f,
                reloadTime: 2f,
                ability: "Overheats after 5 shots with increased damage",
                texture: contentManager.Load<Texture2D>("Weapons/Futuristic_Pistol"),
                speed: 700f));

            weapons.Add(5, new WeaponStats(
                name: "Ion Rifle",
                damage: 20,
                fireCooldown: 0.9f,
                maxAmmo: 100,
                clipSize: 25,
                range: 900f,
                reloadTime: 2.8f,
                ability: "Temporarily disables enemy shields",
                texture: contentManager.Load<Texture2D>("Weapons/Ion_Rifle"),
                speed: 15f));

            weapons.Add(6, new WeaponStats(
                name: "Plasma Blaster",
                damage: 17,
                fireCooldown: 1.0f,
                maxAmmo: 32,
                clipSize: 8,
                range: 600f,
                reloadTime: 3.5f,
                ability: "Delayed explosion causing area damage",
                texture: contentManager.Load<Texture2D>("Weapons/Plasma_Blaster"),
                speed: 15f));

            weapons.Add(7, new WeaponStats(
                name: "Sci-Fi Shotgun",
                damage: 25,
                fireCooldown: 1.8f,
                maxAmmo: 24,
                clipSize: 6,
                range: 300f,
                reloadTime: 3f,
                ability: "Impulse that pushes nearby enemies",
                texture: contentManager.Load<Texture2D>("Weapons/SciFi_Shotgun"),
                speed: 15f));

            weapons.Add(8, new WeaponStats(
                name: "Stealth Handgun",
                damage: 8,
                fireCooldown: 3f,
                maxAmmo: 36,
                clipSize: 9,
                range: 200f,
                reloadTime: 2.2f,
                ability: "Silent shot without revealing position",
                texture: contentManager.Load<Texture2D>("Weapons/Stealth_Handgun"),
                speed: 15f));

            weapons.Add(9, new WeaponStats(
                name: "Tactical Pistol",
                damage: 10,
                fireCooldown: 0.5f,
                maxAmmo: 40,
                clipSize: 10,
                range: 600f,
                reloadTime: 1.8f,
                ability: "Precision mode with extended range",
                texture: contentManager.Load<Texture2D>("Weapons/Tactical_Pistol"),
                speed: 600f));

            // Initialisation de la texture pour les balles
            bulletTexture = new Texture2D(graphicsDevice, 1, 1);
            bulletTexture.SetData(new[] { Color.Red });
        }

        public WeaponStats GetWeapon(int index)
        {
            if (weapons.TryGetValue(index, out var weaponStats))
            {
                return weaponStats;
            }
            throw new KeyNotFoundException($"Weapon with index {index} does not exist.");
        }
    }
}
