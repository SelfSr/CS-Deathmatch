using System;
using UnityEngine;

[System.Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
    public float weight = 1.0f;
}
public class AiWeaponIKBOT : MonoBehaviour
{
    private Transform aimTransform;
    private Transform targetTransform;
    private Animator animator;
    private AiWeapon aiWeapon;
    private Ai_Bot mainBotSript;

    private int iterations = 10;

    [Header("RigSpine")]
    [SerializeField, Range(0, 1)] private float weight = 1.0f;
    private Transform[] boneTransforms;

    [SerializeField] private float angleLimit = 90.0f;
    [SerializeField] private float distanceLimit = 1.5f;
    public Vector3 targetOffset;

    [SerializeField] private HumanBone[] humanBones;

    private void Start()
    {
        animator = GetComponent<Animator>();
        aiWeapon = GetComponent<AiWeapon>();
        mainBotSript = GetComponent<Ai_Bot>();
        boneTransforms = new Transform[humanBones.Length];
        for (int i = 0; i < boneTransforms.Length; i++)
        {
            boneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone);
        }
    }

    private void LateUpdate()
    {
        if (aimTransform == null)
            return;
        if (targetTransform == null)
            return;
        if (mainBotSript.stateMachine.currentState == AiStateId.Death)
            return;
        if (animator.GetBool("Equip") != true)
            return;

        if (aiWeapon.toggleRig)
        {
            Vector3 targetPosition = GetTargetPosition();
            for (int i = 0; i < iterations; i++)
            {
                for (int b = 0; b < boneTransforms.Length; b++)
                {
                    Transform bone = boneTransforms[b];
                    float boneWeight = humanBones[b].weight * weight;
                    AimAtTarget(bone, targetPosition, boneWeight);
                }
            }
        }

    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = (targetTransform.position + targetOffset) - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        float blendOut = 0.0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if (targetAngle > angleLimit)
        {
            blendOut += (targetAngle - angleLimit) / 50.0f;
        }

        float targetDistance = targetDirection.magnitude;
        if (targetDistance < distanceLimit)
        {
            blendOut += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return aimTransform.position + direction;
    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim)
    {
        aimTransform = aim;
    }
}