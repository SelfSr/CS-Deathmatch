// Designed by Kinemation, 2023

using Kinemation.FPSFramework.Runtime.Camera;
using Kinemation.FPSFramework.Runtime.Core.Types;
using Kinemation.FPSFramework.Runtime.FPSAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AiWeapon;

namespace Demo.Scripts.Runtime
{
    public enum OverlayType
    {
        Default,
        Pistol,
        Rifle
    }
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : FPSAnimWeapon
    {
        private FPSController fpsController;

        [Header("Animations")]
        public AnimSequence reloadClip;
        public AnimSequence grenadeClip;
        public AnimSequence fireClip;
        public OverlayType overlayType;

        [Header("Aiming")]
        public bool canAim = true;
        [SerializeField] private List<Transform> scopes;

        [Header("Recoil")]
        public RecoilPattern recoilPattern;
        public FPSCameraShake cameraShake;

        [Header("ShotPlace")]
        public Transform firePoint;
        public AudioClip fire, reload;
        private ParticleSystem shootEffect;

        private AudioSource audioSource;
        private Animator _animator;
        private int _scopeIndex;

        private void Awake()
        {
            fpsController = GetComponentInParent<FPSController>();
        }
        protected void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            audioSource = GetComponent<AudioSource>();
            shootEffect = GetComponentInChildren<ParticleSystem>(firePoint);
        }

        public override Transform GetAimPoint()
        {
            _scopeIndex++;
            _scopeIndex = _scopeIndex > scopes.Count - 1 ? 0 : _scopeIndex;
            return scopes[_scopeIndex];
        }

        public void OnFire()
        {
            if (_animator == null && audioSource == null)
                return;

            shootEffect.Play();
            Vector3 firePointPosition = firePoint.position;
            Quaternion firePointRotation = firePoint.rotation;
            audioSource.PlayOneShot(fire);
            _animator.Play("Fire", 0, 0f);
        }

        public void Reload()
        {
            if (_animator == null && audioSource == null)
                return;
            StartCoroutine(ReloadWeaponAnimation());
        }
        IEnumerator ReloadWeaponAnimation()
        {
            audioSource.PlayOneShot(reload);
            _animator.Rebind();
            _animator.Play("Reload", 0);

            yield return new WaitForSeconds(0.5f);
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                yield return null;
            yield return new WaitForSeconds(0.5f);

            fpsController.ResetActionState();
        }
    }
}