using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RaycastWeapon : MonoBehaviour
{
    public Transform raycast;
    private AudioSource audioSource;

    [Header("Effects")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioClip[] audioClips;
    [HideInInspector] public bool isFiring = false;

    [Header("WeaponSettings")]
    public RuntimeAnimatorController animator;
    public float inaccuracy = 0.4f;
    public float dropMagazineForce = 5f;
    public int magazineSize;
    [SerializeField] private int fireRateInOneSecond = 25;
    [SerializeField] private float damage = 10;
    [SerializeField] private float forceToRidgidbodyAfterEnemyDie = 150;

    private float headDamageMultiply = 2.0f;
    private float limbDamageMultiply = 0.5f;

    [Header("Reload_System")]
    public GameObject magazine;
    [HideInInspector] public int ammoCount;

    private Ray ray;
    private RaycastHit hitInfo;
    private float accumulatedTime;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ammoCount = magazineSize;
    }

    public void StartFiring()
    {
        isFiring = true;
        if (accumulatedTime > 0.0f)
        {
            accumulatedTime = 0.0f;
        }
    }

    public void UpdateFiring(float delta, Vector3 target)
    {
        if (isFiring)
        {
            accumulatedTime += delta;
            float fireInterval = 1.0f / fireRateInOneSecond;
            while (accumulatedTime >= 0.0f)
            {
                FireBullet(target);
                accumulatedTime -= fireInterval;
            }
        }
    }

    private void FireBullet(Vector3 target)
    {
        if (ammoCount <= 0)
            return;
        ammoCount--;

        muzzleFlash.Play();

        ray.origin = raycast.position;
        ray.direction = (target - raycast.position).normalized;

        audioSource.PlayOneShot(audioClips[0]);

        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);

            if (hitInfo.collider.gameObject.CompareTag("BotBodyShot"))
            {
                Vector3 forceDirection = (hitInfo.point - transform.position).normalized;
                DamageEnemy(hitInfo.collider, damage, forceDirection * forceToRidgidbodyAfterEnemyDie, hitInfo.point);
            }

            if (hitInfo.collider.gameObject.CompareTag("BotHeadShot"))
            {
                Vector3 forceDirection = (hitInfo.point - transform.position).normalized;
                DamageEnemy(hitInfo.collider, damage * headDamageMultiply, forceDirection * forceToRidgidbodyAfterEnemyDie, hitInfo.point);
            }

            if (hitInfo.collider.gameObject.CompareTag("BotLimbShot"))
            {
                Vector3 forceDirection = (hitInfo.point - transform.position).normalized;
                DamageEnemy(hitInfo.collider, damage * limbDamageMultiply, forceDirection * forceToRidgidbodyAfterEnemyDie, hitInfo.point);
            }
        }

    }

    private void DamageEnemy(Collider collider, float damage, Vector3 force, Vector3 hitPosition)
    {
        if (collider.GetComponent<HitBox>())
            collider.GetComponent<HitBox>().TakeDamage(damage, force, hitPosition);
        else
            Debug.LogWarning("Script HitBox not found");
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}