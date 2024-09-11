using System;

public class Weapon
{
    public string Name { get; set; }
    public int Damage { get; set; }
    public float FireRate { get; set; } // En tirs par seconde
    public int MaxAmmo { get; set; }
    public int CurrentAmmo { get; set; }
    public float Range { get; set; } // Portée de l'arme
    public string SpecialAbility { get; set; } // Capacité spéciale

    public Weapon(string name, int damage, float fireRate, int maxAmmo, float range, string specialAbility)
    {
        Name = name;
        Damage = damage;
        FireRate = fireRate;
        MaxAmmo = maxAmmo;
        CurrentAmmo = maxAmmo;
        Range = range;
        SpecialAbility = specialAbility;
    }

    // Méthode pour tirer
    public void Fire()
    {
        if (CurrentAmmo > 0)
        {
            CurrentAmmo--;
            // Logique de tir
            Console.WriteLine($"{Name} tire ! Il reste {CurrentAmmo} munitions.");
        }
        else
        {
            Console.WriteLine($"{Name} est à court de munitions.");
        }
    }

    // Méthode pour recharger
    public void Reload()
    {
        CurrentAmmo = MaxAmmo;
        Console.WriteLine($"{Name} rechargé avec succès. Munitions : {CurrentAmmo}/{MaxAmmo}.");
    }

    // Méthode pour utiliser la capacité spéciale
    public void UseSpecialAbility()
    {
        Console.WriteLine($"{Name} utilise sa capacité spéciale : {SpecialAbility}.");
    }
}

