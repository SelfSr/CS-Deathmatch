using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health;
    private void Start()
    {
        OnStart();
    }
    public void TakeDamage(float damage, Vector3 force, Vector3 hitPosition)
    {
        bool debugWriteHP = true;
        health -= damage;
        OnDamage(force, hitPosition);
        if (health <= 0)
        {
            Die(force, hitPosition);
            debugWriteHP = false;
        }
        if (debugWriteHP)
        {
            Debug.Log($"{transform.name} have {health} HP");
        }
    }
    private void Die(Vector3 force, Vector3 hitPosition)
    {
        OnDeath(force, hitPosition);
    }
    protected virtual void OnStart()
    {

    }
    protected virtual void OnDeath(Vector3 force, Vector3 direction)
    {

    }
    protected virtual void OnDamage(Vector3 force, Vector3 direction)
    {

    }
}
