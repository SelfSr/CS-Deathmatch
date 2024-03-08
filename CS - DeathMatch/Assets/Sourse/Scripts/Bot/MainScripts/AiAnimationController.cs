using UnityEngine;

public class AiAnimationController : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    private Ai_Bot mainBotScript;

    private void Start()
    {
        animator = GetComponent<Animator>();
        mainBotScript = GetComponent<Ai_Bot>();
    }

    private void Update()
    {
        if (IsAnimationPlaying("Equip") || IsAnimationPlaying("Holstel") || IsAnimationPlaying("Reload"))
            mainBotScript.navMeshAgent.speed = 0;
        else if (mainBotScript.stateMachine.currentState != AiStateId.Death)
            mainBotScript.navMeshAgent.speed = mainBotScript.botSpeed;

        Vector3 moveDirection = mainBotScript.navMeshAgent.velocity.normalized;
        Vector3 localMoveDirection = transform.InverseTransformDirection(moveDirection);

        float clampedX = Mathf.Clamp(localMoveDirection.x, -1.0f, 1.0f);
        float clampedY = Mathf.Clamp(localMoveDirection.z, -1.0f, 1.0f);

        animator.SetFloat("x", clampedX);
        animator.SetFloat("y", clampedY);
    }

    public bool IsAnimationPlaying(string animationName)
    {
        var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(1);
        if (animatorStateInfo.IsName(animationName))
            return true;

        return false;
    }
}