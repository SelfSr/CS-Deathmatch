using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    private void Start()
    {
        OnStart();
    }

    public void TakeDamage(float damage, Vector3 force, Vector3 hitPosition, bool isPlayer)
    {
        bool debugWriteHP = true;
        health -= damage;
        if (isPlayer)
        {
            float _damage = Mathf.Abs(damage);
            UIEvents.onHealthChanged?.Invoke(health, true);
        }
        OnDamage(force, hitPosition);
        if (health <= 0)
        {
            Die(force, hitPosition);
            debugWriteHP = false;
        }
        if (debugWriteHP)
        {
            //Debug.Log($"{transform.name} have {health} HP");
        }

    }

    public bool IsDead()
    {
        return health <= 0;
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
