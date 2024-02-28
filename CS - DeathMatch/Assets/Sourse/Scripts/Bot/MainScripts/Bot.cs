using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider))]
public class Bot : MonoBehaviour
{
    [Header("CONFIG")]
    public Bot_Settings botConfig;
    [HideInInspector] public Collider thisBotCollaider;

    [Header("VIEW")]
    [HideInInspector] public TargetDetection_Bot targetDetection;
    public Transform enemyTarget;

    [Header("ANIMATION")]
    [HideInInspector] public AnimationController_Bot animationController;
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

    private float health;
    private float currentHealth;

    private void Awake()
    {
        thisBotCollaider = GetComponent<Collider>();
        targetDetection = GetComponentInChildren<TargetDetection_Bot>();
        animationController = GetComponent<AnimationController_Bot>();
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
        stateMachine.RegisterState(new AiAttackEnemy());
        stateMachine.ChangeState(initialState);

        navMeshAgent.speed = botConfig.botSpeed;
    }
    private void Update()
    {
        stateMachine.Update();
        if(Input.GetKeyDown(KeyCode.V))
        {
            stateMachine.ChangeState(AiStateId.AttackEnemy);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            stateMachine.ChangeState(AiStateId.Idle);
        }
    }
    private void OnDrawGizmos()
    {
        if (isDrawRadiusNearBot && transform.position != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, botConfig.searchRadius);
        }
    }
    protected virtual void OnStart()
    {

    }
    protected virtual void OnDeath(Vector3 direction)
    {

    }
    protected virtual void OnDamage(Vector3 direction)
    {

    }
}