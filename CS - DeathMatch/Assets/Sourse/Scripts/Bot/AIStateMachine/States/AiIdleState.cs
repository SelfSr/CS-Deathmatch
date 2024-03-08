using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdleState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Idle;
    }
    public void Enter(Ai_Bot bot)
    {
        bot.aiWeapon.DeactivateWeapon();
    }
    public void Update(Ai_Bot bot)
    {
        bot.navMeshAgent.speed = 0;
    }
    public void Exit(Ai_Bot bot)
    {
    }
}
