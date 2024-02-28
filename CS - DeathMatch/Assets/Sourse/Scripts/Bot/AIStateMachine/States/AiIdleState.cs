using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdleState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Idle;
    }
    public void Enter(Bot bot)
    {
    }
    public void Update(Bot bot)
    {
        bot.navMeshAgent.speed = 0;
    }
    public void Exit(Bot bot)
    {
    }
}
