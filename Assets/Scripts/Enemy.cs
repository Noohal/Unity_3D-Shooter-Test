using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Target
{
    public float health { get; set; } = 100f;
    public bool isDead { get; set; } = false;
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        //Debug.Log(health);
        if (health <= 0)
        {
            isDead = true;
        }

        //if (isDead) Die();
    }

    public void Die()
    {
        Destroy(gameObject);
        //Debug.Log("DEAD");
    }


}
