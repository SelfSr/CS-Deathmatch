using UnityEngine;

public class AiDeathState : AiState
{
    public Vector3 force;
    public Vector3 hitPosition;

    public AiStateId GetId()
    {
        return AiStateId.Death;
    }
    public void Enter(Bot bot)
    {
        bot.ragdoll.ActivateRagdoll();
        bot.thisBotCollaider.enabled = false;
        bot.aiWeapon.DisableWeapon();
        bot.ragdoll.Hit(force, hitPosition);
        bot.navMeshAgent.speed = 0;
    }
    public void Update(Bot bot)
    {
    }
    public void Exit(Bot bot)
    {
    }
}
