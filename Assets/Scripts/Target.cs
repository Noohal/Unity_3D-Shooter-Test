using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Target
{
    public float health { get; set; }
    public bool isDead { get; set; }

    public void TakeDamage(float damage);
    void Die();
}
