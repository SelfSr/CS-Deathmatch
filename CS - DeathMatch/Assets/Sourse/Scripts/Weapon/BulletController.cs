using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float timeToDestroy = 2f;

    private void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        Invoke("Destroy", timeToDestroy);
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
