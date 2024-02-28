using Demo.Scripts.Runtime;
using Kinemation.FPSFramework.Runtime.Core.Components;
using UnityEngine;

public class PlayerHealth : Health
{
    private Ragdoll ragdoll;
    private FPSController FPSController;
    private CoreAnimComponent CoreAnimComponent;
    [SerializeField] private GameObject weaponBone;
    protected override void OnStart()
    {
        ragdoll = GetComponent<Ragdoll>();
        ragdoll.DeactivateRagdoll();
        FPSController = GetComponent<FPSController>();
        CoreAnimComponent = GetComponent<CoreAnimComponent>();
    }
    protected override void OnDeath(Vector3 force, Vector3 direction)
    {
        ragdoll.ActivateRagdoll();
        FPSController.enabled = false;
        CoreAnimComponent.enabled = false;
        weaponBone.gameObject.SetActive(false);
    }
    protected override void OnDamage(Vector3 force, Vector3 direction)
    {

    }
}
