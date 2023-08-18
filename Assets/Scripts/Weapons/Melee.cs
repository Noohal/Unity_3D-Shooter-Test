using UnityEngine;

[CreateAssetMenu(fileName = "Melee", menuName = "Weapon/Melee")]
public class Melee : ScriptableObject
{
    public new string name;
    public Transform prefab;
    public Transform handsPrefab;

    public float damage;
}