using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunObject : ScriptableObject
{
    // Basic Info
    public string Name;
    public Transform prefab;
    //public Transform handsPrefab;

    // Firing Properties
    public int magazineCapacity;
    public int maxAmmo;
    public int currentAmmo;
    public float reloadTime;
    public float fireRate;
    public float range;
    public float recoilResetTime;
    public Vector3[] recoilPattern = new Vector3[1];

    // Damage
    public float damage;
}
