public enum AiStateId
{
    Idle,
    WanderingMap,
    Death,
    AttackEnemy
}
public interface AiState
{
    AiStateId GetId();
    void Enter(Bot bot);
    void Update(Bot bot);
    void Exit(Bot bot);
}
