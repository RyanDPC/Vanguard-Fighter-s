public class WeaponsLibrary
{
    public Weapon AdvancedAssaultRifle;
    public Weapon CompactSidearm;
    public Weapon EnergyRifle;
    public Weapon FuturisticPistol;
    public Weapon IonRifle;
    public Weapon PlasmaBlaster;
    public Weapon SciFiShotgun;
    public Weapon StealthHandgun;
    public Weapon TacticalPistol;

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
        AdvancedAssaultRifle.Fire();
        AdvancedAssaultRifle.Reload();
        AdvancedAssaultRifle.UseSpecialAbility();

        CompactSidearm.Fire();
        CompactSidearm.Reload();
        CompactSidearm.UseSpecialAbility();

        // ... Teste les autres armes ici de manière similaire
    }
}
