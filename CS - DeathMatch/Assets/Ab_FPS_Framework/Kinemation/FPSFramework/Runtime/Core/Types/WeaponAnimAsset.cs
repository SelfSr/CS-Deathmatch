// Designed by KINEMATION, 2023

using Kinemation.FPSFramework.Runtime.Recoil;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kinemation.FPSFramework.Runtime.Core.Types
{
    [System.Serializable, CreateAssetMenu(fileName = "NewWeaponAsset", menuName = "FPS Animator/WeaponAnimAsset")]
    public class WeaponAnimAsset : ScriptableObject
    {
        [Header("General"), Tooltip("Adjusts weapon model rotation")]
        public Quaternion rotationOffset = Quaternion.identity;
        public AimOffsetTable aimOffsetTable;
        public RecoilAnimData recoilData;
        public AnimSequence overlayPose;

        [Tooltip("Defines weapon default position and rotation pose.")]
        public LocRot weaponBone = LocRot.identity;

        [Header("AdsLayer")]
        public AdsData adsData;

        [Tooltip("Offsets the arms pose")]
        public ViewmodelOffset viewmodelOffset = new ViewmodelOffset()
        {
            poseOffset = LocRot.identity,
            rightHandOffset = LocRot.identity,
            leftHandOffset = LocRot.identity
        };

        public LocRot viewOffset = LocRot.identity;

        [FormerlySerializedAs("springData")]
        [Header("SwayLayer")]
        public LocRotSpringData aimSwaySettings;
        [FormerlySerializedAs("freeAimData")] public FreeAimData freeAimSettings;
        [FormerlySerializedAs("moveSwayData")] public MoveSwayData moveSwaySettings;

        [Header("Weapon Collision")]
        public GunBlockData blockData;

        [Header("Pivoting")]
        public Vector3 adsRecoilOffset;
        public Vector3 adsSwayOffset;

        [Header("Damage")]
        [Tooltip("Main Damage")]
        public float BodyDamage;

        [Range(1f, 3f)]
        public float headDamageMultiply = 1f;

        [Range(0.1f, 1f)]
        public float limbDamageMultiply = 0.5f;

        [Header("Force After Enemy Die")]
        public float force = 200;

        [Header("WallBane")]
        [Range(0f, 5f)]
        public float distanceWallBane = 1;

        [Header("Crosshair")]
        [Range(1f, 1000f)]
        public float crosshairSpread = 200;
    }
}