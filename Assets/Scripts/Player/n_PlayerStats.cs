using UnityEngine;

public class n_PlayerStats : MonoBehaviour, Target
{

    public float health { get; set; } = 100f;
    public int killCount { get; set; } = 0;
    public bool isDead { get; set; } = false;

    // Movement
    public float runningMoveSpeed { get; private set; } = 8f;
    public float walkingMoveSpeed { get; private set; } = 4f;
    public float crouchingMoveSpeed { get; private set; } = 2f;
    public float gravity { get; private set; } = -21f;
    public float jumpHeight { get; private set; } = 3f;

    public float standingHeight { get; private set; } = 2f;
    public float crouchHeight { get; private set; } = 1f;
    public float cameraStandingHeight { get; private set; } = 0.6f;
    public float cameraCrouchHeight { get; private set; } = 0.3f;
    public float crouchSpeed { get; private set; } = 10f;

    public float mouseSensitivity { get; private set; } = 200f;

    // Methods
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
