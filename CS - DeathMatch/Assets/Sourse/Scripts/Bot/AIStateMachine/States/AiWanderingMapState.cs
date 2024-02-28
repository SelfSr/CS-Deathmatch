using UnityEngine;
using UnityEngine.AI;

public class AiWanderingMapState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.WanderingMap;
    }
    public void Enter(Bot bot)
    {

    }
    public void Update(Bot bot)
    {
        if (!bot.navMeshAgent.pathPending && bot.navMeshAgent.remainingDistance < 0.3f)
        {
            bool getCorrectPoint = false;
            while (!getCorrectPoint)
            {
                NavMeshHit hit;
                NavMesh.SamplePosition(Random.insideUnitSphere * bot.botConfig.searchRadius + bot.transform.position, out hit, bot.botConfig.searchRadius, NavMesh.AllAreas);
                bot.randomPoint = hit.position;

                bot.navMeshAgent.CalculatePath(bot.randomPoint, bot.navMeshPath);

                if (bot.navMeshPath.status == NavMeshPathStatus.PathComplete)
                    getCorrectPoint = true;
            }
            bot.navMeshAgent.SetDestination(bot.randomPoint);
        }
    }
    public void Exit(Bot bot)
    {
    }
}
