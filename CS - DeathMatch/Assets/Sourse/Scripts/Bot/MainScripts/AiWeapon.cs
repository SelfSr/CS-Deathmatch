using System.Collections;
using UnityEngine;

[RequireComponent(typeof(WeaponIKBOT))]
public class AiWeapon : MonoBehaviour
{
    private Animator animator;
    private MeshSockets sockets;
    private WeaponIKBOT weaponIKBOT;
    private Transform currentTarget;
    private Transform currentWeapon;
    private WeaponBOTScript weaponBotScript;

    [SerializeField] private int selectedWeapon = 1;
    [SerializeField] private float inaccuracy = 0.0f;
    [SerializeField] private Transform[] weapons;
    [HideInInspector] public bool toggleRig;

    private bool weaponActive = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sockets = GetComponent<MeshSockets>();
        weaponIKBOT = GetComponent<WeaponIKBOT>();

        currentWeapon = weapons[selectedWeapon - 1];
        weaponBotScript = currentWeapon.GetComponent<WeaponBOTScript>();

        sockets.Attach(currentWeapon, MeshSockets.SocketId.Spine);
        currentWeapon.gameObject.SetActive(true);
    }
    private void Update()
    {
        if (currentTarget && currentWeapon && weaponActive)
        {
            Vector3 target = currentTarget.position + weaponIKBOT.targetOffset;
            target += Random.insideUnitSphere * inaccuracy;
            weaponBotScript.UpdateFiring(Time.deltaTime, target);
        }
    }
    public void SetFiring(bool enabled)
    {
        if (enabled)
            weaponBotScript.StartFiring();
        else
            weaponBotScript.StopFiring();
    }
    public void ActivateWeapon() => StartCoroutine(EquipWeaopn());
    IEnumerator EquipWeaopn()
    {
        animator.SetBool("Equip", true);
        yield return new WaitForSeconds(0.5f);
        weaponIKBOT.SetAimTransform(currentWeapon.GetComponent<WeaponBOTScript>().raycast);
    }
    public void DeactivateWeapon()
    {
        animator.SetBool("Equip", false);
    }
    public void DisableWeapon() => weapons[selectedWeapon - 1].gameObject.SetActive(false);
    public void OnAnimationEvent(string eventName)
    {
        if (eventName == "equipWeapon")
            sockets.Attach(currentWeapon, MeshSockets.SocketId.RightHand);

        if (eventName == "removeWeapon")
            sockets.Attach(currentWeapon, MeshSockets.SocketId.Spine);

        if (eventName == "EnableRig")
        {
            toggleRig = true;
            weaponActive = true;
        }

        if (eventName == "DisableRig")
        {
            toggleRig = false;
            weaponActive = false;
        }
    }
    public void SetTarget(Transform target)
    {
        weaponIKBOT.SetTargetTransform(target);
        currentTarget = target;
    }
    public void SetWeaponAimTransform()
    {
        weaponIKBOT.SetAimTransform(currentWeapon.GetComponent<WeaponBOTScript>().raycast);
    }
}