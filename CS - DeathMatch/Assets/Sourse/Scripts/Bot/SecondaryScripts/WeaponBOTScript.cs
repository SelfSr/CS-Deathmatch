using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WeaponBOTScript : MonoBehaviour
{
    [HideInInspector] public Transform raycast;
    private AudioSource audioSource;

    [Header("Effects")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private TrailRenderer bulletTracer;
    [SerializeField] private AudioClip[] audioClips;
    public bool isFiring = false;

    [Header("WeaponSettings")]
    [SerializeField] private int fireRateInOneSecond = 25;
    [SerializeField] private float damage = 10;
    [SerializeField] private float force = 150;

    private Ray ray;
    private RaycastHit hitInfo;
    private float accumulatedTime;

    private void Start()
    {
        raycast = GameObject.Find("Raycast").transform;
        audioSource = GetComponent<AudioSource>();
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
        accumulatedTime += delta;
        float fireInterval = 1.0f / fireRateInOneSecond;
        while (accumulatedTime >= 0.0f)
        {
            FireBullet(target);
            accumulatedTime -= fireInterval;
        }
    }

    private void FireBullet(Vector3 target)
    {
        muzzleFlash.Play();

        ray.origin = raycast.position;
        ray.direction = (target - raycast.position).normalized;

        var tracer = Instantiate(bulletTracer, ray.origin, Quaternion.identity);
        tracer.AddPosition(ray.origin);
        audioSource.PlayOneShot(audioClips[0]);

        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
            tracer.transform.position = hitInfo.point;

            if (hitInfo.collider.gameObject.CompareTag("BotBodyShot"))
            {
                Vector3 forceDirection = (hitInfo.point - transform.position).normalized;
                DamageBot(hitInfo.collider, damage, forceDirection * force, hitInfo.point);
            }
        }

    }
    private void DamageBot(Collider collider, float damage, Vector3 force, Vector3 hitPosition)
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
