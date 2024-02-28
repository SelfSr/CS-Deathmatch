using System.Linq;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Animator animator;
    private Rigidbody[] rigidbodies;

    private Collider[] colliders;
    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        foreach (var collaider in colliders)
        {
            collaider.gameObject.AddComponent<HitBox>();
        }

        DeactivateRagdoll();
    }
    public void ActivateRagdoll()
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }
        animator.enabled = false;
    }
    public void DeactivateRagdoll()
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
        animator.enabled = true;
    }
    public void Hit(Vector3 force, Vector3 hitPosition)
    {
        Rigidbody injuredRidgidbody = rigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, hitPosition)).First();
        injuredRidgidbody.AddForceAtPosition(force, hitPosition, ForceMode.Impulse);
    }
}