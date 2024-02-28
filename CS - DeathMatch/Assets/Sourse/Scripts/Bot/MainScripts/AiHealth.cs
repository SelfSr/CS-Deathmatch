using UnityEngine;

public class AiHealth : Health
{
    private Bot mainBotScript;
    protected override void OnStart()
    {
        mainBotScript = GetComponent<Bot>();
    }
    protected override void OnDeath(Vector3 force, Vector3 direction)
    {
        AiDeathState deathState = mainBotScript.stateMachine.GetState(AiStateId.Death) as AiDeathState;
        deathState.force = force;
        deathState.hitPosition = direction;
        mainBotScript.stateMachine.ChangeState(AiStateId.Death);
    }
    protected override void OnDamage(Vector3 force, Vector3 direction)
    {

    }
}