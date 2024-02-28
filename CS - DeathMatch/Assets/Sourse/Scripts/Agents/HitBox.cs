using UnityEngine;

public class HitBox : MonoBehaviour
{
    private Health health;
    private void OnEnable()
    {
        health = GetComponentInParent<Health>();
    }
    public void TakeDamage(float damage, Vector3 force, Vector3 hitPosition)
    {
        health.TakeDamage(damage, force, hitPosition);
    }
}
