using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Bot : MonoBehaviour
{
    public float health = 100;

    private TargetDetection targetDetection;
    private MovementController movementController;

    private Coroutine detectEnemy;

    private void Start()
    {
        targetDetection = GetComponentInChildren<TargetDetection>();
        movementController = GetComponent<MovementController>();
    }

    private void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

    }
}