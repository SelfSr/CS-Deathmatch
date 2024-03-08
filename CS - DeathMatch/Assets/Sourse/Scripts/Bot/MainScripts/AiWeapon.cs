using cowsins;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AiWeaponIKBOT))]
public class AiWeapon : MonoBehaviour
{
    public enum WeaponState
    {
        Holstering,
        Holstered,
        Activating,
        Active,
        Reloading
    }
    public enum WeaponSlot
    {
        Primary,
        Secondary
    }

    [HideInInspector]
    public RaycastWeapon currentWeapon
    {
        get { return weapons[current]; }
    }

    public WeaponSlot currentWeaponSlot
    {
        get { return (WeaponSlot)current; }
    }

    public RaycastWeapon[] weapons = new RaycastWeapon[2];
    private int current = 0;

    private Animator animator;
    private MeshSockets sockets;
    private AiWeaponIKBOT weaponIKBOT;
    private Transform currentTarget;
    WeaponState weaponState = WeaponState.Holstered;

    private GameObject magazineHand;

    [HideInInspector] public bool toggleRig;

    public bool IsActive()
    {
        return weaponState == WeaponState.Active;
    }

    public bool IsHolstered()
    {
        return weaponState == WeaponState.Holstered;
    }

    public bool IsReloading()
    {
        return weaponState == WeaponState.Reloading;
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        weaponIKBOT = GetComponent<AiWeaponIKBOT>();
    }

    private void Start()
    {
        sockets = GetComponent<MeshSockets>();
        if (weapons.Length != 0)
        {
            sockets.Attach(weapons[0].transform, MeshSockets.SocketId.Spine);
            sockets.Attach(weapons[1].transform, MeshSockets.SocketId.RightLeg);
            foreach (var weapon in weapons)
                weapon.gameObject.SetActive(true);
        }
        weaponIKBOT.SetAimTransform(currentWeapon.raycast);
    }

    private void Update()
    {
        if (currentTarget && currentWeapon && IsActive())
        {
            Vector3 target = currentTarget.position + weaponIKBOT.targetOffset;
            target += Random.insideUnitSphere * currentWeapon.inaccuracy;
            currentWeapon.UpdateFiring(Time.deltaTime, target);
        }
    }

    public void SetFiring(bool enabled)
    {
        if (enabled)
            currentWeapon.StartFiring();
        else
            currentWeapon.StopFiring();
    }

    public void ActivateWeapon()
    {
        StartCoroutine(EquipWeaponAnimation());
    }

    public void DeactivateWeapon()
    {
        SetTarget(null);
        SetFiring(false);
        StartCoroutine(HolsterWeaponAnimation());
    }

    public void ReloadWeapon()
    {
        if (IsActive())
            StartCoroutine(ReloadWeaponAnimation());
    }

    public void SwitchWeapon(WeaponSlot slot)
    {
        if (IsHolstered())
        {
            current = (int)slot;
            ActivateWeapon();
            return;
        }

        int equipIndex = (int)slot;
        if (IsActive() && current != equipIndex)
            StartCoroutine(SwitchWeaponAnimation(equipIndex));
    }

    public void DisableWeapons()
    {
        foreach (var weapon in weapons)
            weapon.gameObject.SetActive(false);
    }

    public void SetTarget(Transform target)
    {
        weaponIKBOT.SetTargetTransform(target);
        currentTarget = target;
    }

    IEnumerator EquipWeaponAnimation()
    {
        weaponState = WeaponState.Activating;
        animator.runtimeAnimatorController = currentWeapon.animator;
        animator.SetBool("Equip", true);

        yield return new WaitForSeconds(0.5f);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
            yield return null;

        weaponIKBOT.enabled = true;
        weaponIKBOT.SetAimTransform(currentWeapon.raycast);
        weaponState = WeaponState.Active;
    }

    IEnumerator HolsterWeaponAnimation()
    {
        weaponState = WeaponState.Holstering;
        animator.SetBool("Equip", false);

        weaponIKBOT.enabled = false;
        yield return new WaitForSeconds(0.5f);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
            yield return null;
        weaponState = WeaponState.Holstered;
    }

    IEnumerator ReloadWeaponAnimation()
    {
        weaponState = WeaponState.Reloading;
        animator.runtimeAnimatorController = currentWeapon.animator;
        animator.SetTrigger("reload_weapon");
        weaponIKBOT.enabled = false;

        yield return new WaitForSeconds(0.5f);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
            yield return null;

        weaponIKBOT.SetAimTransform(currentWeapon.raycast);
        weaponIKBOT.enabled = true;
        weaponState = WeaponState.Active;
    }

    IEnumerator SwitchWeaponAnimation(int index)
    {
        yield return StartCoroutine(HolsterWeaponAnimation());
        current = index;
        weaponIKBOT.SetAimTransform(currentWeapon.raycast);
        yield return StartCoroutine(EquipWeaponAnimation());
    }

    public void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "detach_magazine":
                DetachMagazine();
                break;
            case "drop_magazine":
                DropMagazine();
                break;
            case "refill_magazine":
                RefillMagazine();
                break;
            case "attach_magazine":
                AttachMagazine();
                break;
        }

        if (eventName == "equipWeapon")
            sockets.Attach(currentWeapon.transform, MeshSockets.SocketId.RightHand);

        if (eventName == "removeWeapon")
        {
            if (current == 0)
                sockets.Attach(currentWeapon.transform, MeshSockets.SocketId.Spine);
            else
                sockets.Attach(currentWeapon.transform, MeshSockets.SocketId.RightLeg);
        }

        if (eventName == "EnableRig")
            toggleRig = true;

        if (eventName == "DisableRig")
            toggleRig = false;
    }

    private void DetachMagazine()
    {
        var leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
        magazineHand = Instantiate(currentWeapon.magazine, leftHand, true);
        currentWeapon.magazine.SetActive(false);
    }

    private void DropMagazine()
    {
        GameObject droppedMagazine = Instantiate(magazineHand, magazineHand.transform.position, magazineHand.transform.rotation);
        droppedMagazine.SetActive(true);
        Rigidbody body = droppedMagazine.AddComponent<Rigidbody>();
        droppedMagazine.AddComponent<DestroyMe>().timeToDestroy = 5;

        Vector3 dropDirection = -gameObject.transform.right;
        dropDirection += Vector3.down;

        body.AddForce(dropDirection * currentWeapon.dropMagazineForce, ForceMode.Impulse);
        droppedMagazine.AddComponent<BoxCollider>();
        magazineHand.SetActive(false);
    }

    private void RefillMagazine()
    {
        magazineHand.SetActive(true);
    }

    private void AttachMagazine()
    {
        currentWeapon.magazine.SetActive(true);
        Destroy(magazineHand);
        currentWeapon.ammoCount = currentWeapon.magazineSize;
        animator.ResetTrigger("reload_weapon");
    }
}