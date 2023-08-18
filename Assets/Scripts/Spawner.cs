using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;

    Enemy currentEnemy;
    // Start is called before the first frame update
    void Start()
    {
        currentEnemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentEnemy.isDead && currentEnemy != null)
        {
            StartCoroutine(KillEnemy());
        }
    }

    void SpawnEnemy()
    {
        currentEnemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
    }

    IEnumerator KillEnemy()
    {
        Destroy (currentEnemy.gameObject);
        yield return new WaitForSeconds(2f);
        SpawnEnemy();
    }
}
