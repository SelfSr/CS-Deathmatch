using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AiSensor))]
[RequireComponent(typeof(AiTargetingSystem))]
[RequireComponent(typeof(AiAnimationController))]
[RequireComponent(typeof(Ragdoll))]
[RequireComponent(typeof(AiWeapon))]
[RequireComponent(typeof(NavMeshAgent))]

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MeshSockets))]
public class Ai_Bot : MonoBehaviour
{
    [Header("MAIN SETTINGS")]
    public bool isCanShoot = true;
    public float botSpeed = 3;
    public float searchRadius = 20;
    public float stoppingDistance = 10;
    public float closeDistance = 5;
    public float shootingDelayDuration = 1f;

    [HideInInspector] public Collider thisBotCollaider;

    [Header("VIEW")]
    [HideInInspector] public AiSensor sensor;
    [HideInInspector] public AiTargetingSystem targetingSystem;

    [Header("ANIMATION")]
    [HideInInspector] public AiAnimationController animationController;
    [HideInInspector] public Ragdoll ragdoll;

    [Header("LOCOMOTION")]
    [SerializeField] private bool isDrawRadiusNearBot = true;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public NavMeshPath navMeshPath;
    [HideInInspector] public Vector3 randomPoint;

    [Header("State Machine")]
    public AiStateMachine stateMachine;
    public AiStateId initialState;

    [Header("Weapons")]
    [HideInInspector] public AiWeapon aiWeapon;

    private void Awake()
    {
        thisBotCollaider = GetComponent<Collider>();
        sensor = GetComponent<AiSensor>();
        targetingSystem = GetComponent<AiTargetingSystem>();
        animationController = GetComponent<AiAnimationController>();
        ragdoll = GetComponent<Ragdoll>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        aiWeapon = GetComponentInChildren<AiWeapon>();
        navMeshPath = new NavMeshPath();

    }

    private void Start()
    {
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiWanderingMapState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiAttackTargetState());
        stateMachine.RegisterState(new AiFindTargetState());
        stateMachine.ChangeState(initialState);

        navMeshAgent.speed = botSpeed;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void OnDrawGizmos()
    {
        if (isDrawRadiusNearBot && transform.position != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, searchRadius);
        }
    }
}