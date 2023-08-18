using UnityEngine;

public class JettBladestorm : n_GunObject
{
    public string Name { get; set; } = "Bladestorm";
    public Transform gunPrefab { get; set; }

    public int currentAmmo { get; set; }
    public int reserveAmmo { get; set; }
    public int magazineCapacity { get; set; } = 5;
    public int maximumAmmo { get; set; } = 5;

    public float damage { get; set; } = 100f;

    public float fireRate { get; set; }
    public float fireRateRPM { get; set; } = 360f;
    public float fireRange { get; set; } = 25f;
    public float reloadTime { get; set; } = 5f;
    public float recoilResetTime { get; set; } = 0.5f;

    public Vector3[] recoilPattern { get; set; } = new Vector3[] { };

    public void InitializeGun()
    {
        currentAmmo = magazineCapacity;
        reserveAmmo = maximumAmmo;

        fireRate = ConvertFireRate();
    }

    public int ConvertFireRate()
    {
        return (int)fireRateRPM / 60;
    }
}