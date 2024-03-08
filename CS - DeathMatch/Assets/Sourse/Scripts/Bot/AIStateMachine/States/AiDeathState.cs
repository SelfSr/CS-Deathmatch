using UnityEngine;

public class AiDeathState : AiState
{
    public Vector3 force;
    public Vector3 hitPosition;

    public AiStateId GetId()
    {
        return AiStateId.Death;
    }
    public void Enter(Ai_Bot bot)
    {
        bot.navMeshAgent.speed = 0;

        bot.aiWeapon.SetTarget(null);
        bot.aiWeapon.SetFiring(false);
        bot.ragdoll.ActivateRagdoll();
        bot.aiWeapon.DisableWeapons();
        bot.thisBotCollaider.enabled = false;

        bot.sensor.enabled = false;

        bot.navMeshAgent.ResetPath();

        bot.ragdoll.Hit(force, hitPosition);
    }
    public void Update(Ai_Bot bot)
    {
    }
    public void Exit(Ai_Bot bot)
    {
    }
}
