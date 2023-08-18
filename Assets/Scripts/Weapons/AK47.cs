using UnityEngine;

public class AK47 : MonoBehaviour, n_GunObject
{
    public string Name { get; set; } = "AK-47";
    [SerializeField] private Transform _gunPrefab;
    public Transform gunPrefab { 
        get { return _gunPrefab; } 
        set { _gunPrefab = value; } 
    }

    public int currentAmmo { get; set; }
    public int reserveAmmo { get; set; }
    public int magazineCapacity { get; set; } = 30;
    public int maximumAmmo { get; } = 90; 

    public float damage { get; set; } = 35f;

    public float fireRate { get; set; }
    public float fireRateRPM { get; set; } = 600f;
    public float fireRange { get; set; } = 20f;
    public float reloadTime { get; set; } = 2.4f;
    public float recoilResetTime { get; set; } = 0.5f;

    public Vector3[] recoilPattern { get; set; } = new Vector3[]
    {
        new Vector3(-0.5f, 0, 0), // 1
        new Vector3(-0.5f, 0, 0), // 2
        new Vector3(-0.8f, 0, 0), // 3
        new Vector3(-0.8f, 0, 0), // 4
        new Vector3(-0.8f, 0, 0), // 5
        new Vector3(-0.8f, 0, 0), // 6
        new Vector3(-0.8f, 0, 0), // 7
        new Vector3(-0.8f, 0, 0), // 8
        new Vector3(-0.8f, 0, 0), // 9
        new Vector3(-0.8f, 0, 0), // 10
        new Vector3(-0.8f, 0, 0), // 11
        new Vector3(-0.8f, 0, 0), // 12
        new Vector3(0, 0.8f, 0),  // 13
        new Vector3(0, 0.8f, 0),  // 14
        new Vector3(0, 0.8f, 0),  // 15
        new Vector3(0, 0.8f, 0),  // 16
        new Vector3(0, -0.8f, 0), // 17
        new Vector3(0, -0.8f, 0), // 18
        new Vector3(0, -0.8f, 0), // 19
        new Vector3(0, -0.8f, 0), // 20
        new Vector3(0, -0.8f, 0), // 21
        new Vector3(0, -0.8f, 0), // 22
        new Vector3(0, -0.8f, 0), // 23
        new Vector3(0, -0.8f, 0), // 24
        new Vector3(0, -0.8f, 0), // 25
    };
    /*
        new Vector3(-0.5f, 0, 0), // 1
        new Vector3(-0.5f, 0, 0), // 2
        new Vector3(-0.8f, 0, 0), // 3
        new Vector3(-0.8f, 0, 0), // 4
        new Vector3(-0.8f, 0, 0), // 5
        new Vector3(-0.8f, 0, 0), // 6
        new Vector3(-0.8f, 0, 0), // 7
        new Vector3(-0.8f, 0, 0), // 8
        new Vector3(-0.8f, 0, 0), // 9
        new Vector3(-0.8f, 0, 0), // 10
        new Vector3(-0.8f, 0, 0), // 11
        new Vector3(-0.8f, 0, 0), // 12
        new Vector3(0, 0.8f, 0),  // 13
        new Vector3(0, 0.8f, 0),  // 14
        new Vector3(0, 0.8f, 0),  // 15
        new Vector3(0, 0.8f, 0),  // 16
        new Vector3(0, -0.8f, 0), // 17
        new Vector3(0, -0.8f, 0), // 18
        new Vector3(0, -0.8f, 0), // 19
        new Vector3(0, -0.8f, 0), // 20
        new Vector3(0, -0.8f, 0), // 21
        new Vector3(0, -0.8f, 0), // 22
        new Vector3(0, -0.8f, 0), // 23
        new Vector3(0, -0.8f, 0), // 24
        new Vector3(0, -0.8f, 0), // 25
    */

    public void InitializeGun()
    {
        currentAmmo = magazineCapacity;
        reserveAmmo = maximumAmmo;

        fireRate = ConvertFireRate();
    }

    public int ConvertFireRate()
    {
        return (int)(fireRateRPM / 60);
    }
}