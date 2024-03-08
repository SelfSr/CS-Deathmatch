using UnityEngine;
using UnityEngine.AI;

public class AiFindTargetState : AiState
{
    int maxAttempts = 10;
    bool getCorrectPoint = false;

    public AiStateId GetId()
    {
        return AiStateId.FindTarget;
    }

    public void Enter(Ai_Bot bot)
    {
        bot.navMeshAgent.speed = bot.botSpeed;
        bot.aiWeapon.ActivateWeapon();
    }

    public void Update(Ai_Bot bot)
    {
        if (bot.targetingSystem.HasTarget)
        {
            bot.stateMachine.ChangeState(AiStateId.AttackTarget);
        }

        if (!bot.navMeshAgent.pathPending && bot.navMeshAgent.remainingDistance < 0.3f)
        {
            bool getCorrectPoint = false;
            while (!getCorrectPoint)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(Random.insideUnitSphere * bot.searchRadius + bot.transform.position, out hit, bot.searchRadius, NavMesh.AllAreas))
                {
                    bot.randomPoint = hit.position;
                    bot.navMeshAgent.CalculatePath(bot.randomPoint, bot.navMeshPath);
                    if (bot.navMeshPath.status == NavMeshPathStatus.PathComplete)
                        getCorrectPoint = true;
                }
                else
                {
                    Debug.LogError("not correct point");
                }
            }
            bot.navMeshAgent.SetDestination(bot.randomPoint);
        }

    }

    public void Exit(Ai_Bot bot)
    {
    }
}
