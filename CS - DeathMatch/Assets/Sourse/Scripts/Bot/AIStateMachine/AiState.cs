public enum AiStateId
{
    Idle,
    WanderingMap,
    Death,
    AttackTarget,
    FindTarget
}
public interface AiState
{
    AiStateId GetId();
    void Enter(Ai_Bot bot);
    void Update(Ai_Bot bot);
    void Exit(Ai_Bot bot);
}
