using Demo.Scripts.Runtime;
using Kinemation.FPSFramework.Runtime.Core.Components;
using UnityEngine;

public class PlayerHealth : Health
{
    private Ragdoll ragdoll;
    private FPSController fpsController;
    private CoreAnimComponent coreAnimComponent;
    private FPSMovement fpsMovement;
    [SerializeField] private GameObject weaponBone;
    protected override void OnStart()
    {
        ragdoll = GetComponent<Ragdoll>();
        ragdoll.DeactivateRagdoll();
        fpsController = GetComponent<FPSController>();
        fpsMovement = GetComponent<FPSMovement>();
        coreAnimComponent = GetComponent<CoreAnimComponent>();
    }
    protected override void OnDeath(Vector3 force, Vector3 direction)
    {
        ragdoll.ActivateRagdoll();
        fpsController.enabled = false;
        fpsMovement.enabled = false;
        coreAnimComponent.enabled = false;
        weaponBone.gameObject.SetActive(false);
    }
    protected override void OnDamage(Vector3 force, Vector3 direction)
    {

    }
}
