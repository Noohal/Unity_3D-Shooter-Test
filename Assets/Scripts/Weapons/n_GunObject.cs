using UnityEngine;

public interface n_GunObject
{
    public string Name { get; set; }
    public Transform gunPrefab { get; set; }

    public int currentAmmo { get; set; }
    public int reserveAmmo { get; set; }
    public int magazineCapacity { get; set; }
    public int maximumAmmo { get; }

    public float damage { get; set; }

    public float fireRate { get; set; } // rounds per second
    public float fireRateRPM { get; set; }
    public float fireRange { get; set; }
    public float reloadTime { get; set; }
    public float recoilResetTime { get; set; }

    public Vector3[] recoilPattern { get; set; }

    public void InitializeGun();

    int ConvertFireRate();
}
