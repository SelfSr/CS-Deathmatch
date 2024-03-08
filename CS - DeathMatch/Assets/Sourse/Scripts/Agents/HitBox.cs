using UnityEngine;

public class HitBox : MonoBehaviour
{
    private Health health;
    private CharacterController characterController;

    private bool isPlayer = false;
    private void OnEnable()
    {
        health = GetComponentInParent<Health>();
        characterController = GetComponentInParent<CharacterController>();
        if (characterController)
            isPlayer = true;
    }
    public void TakeDamage(float damage, Vector3 force, Vector3 hitPosition)
    {
        health.TakeDamage(damage, force, hitPosition, isPlayer);
    }
}
