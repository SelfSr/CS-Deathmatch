using UnityEngine.Animations.Rigging;

public class AiStateMachine
{
    public AiState[] states;
    public Ai_Bot Bot;
    public AiStateId currentState;

    public AiStateMachine(Ai_Bot bot)
    {
        this.Bot = bot;
        int numStates = System.Enum.GetNames(typeof(AiStateId)).Length;
        states = new AiState[numStates];
    }
    public void RegisterState(AiState state)
    {
        int index = (int)state.GetId();
        states[index] = state;
    }
    public AiState GetState(AiStateId stateId)
    {
        int index = (int)stateId;
        return states[index];
    }
    public void Update()
    {
        GetState(currentState)?.Update(Bot);
    }
    public void ChangeState(AiStateId newState)
    {
        GetState(currentState)?.Exit(Bot);
        currentState = newState;
        GetState(currentState)?.Enter(Bot);
    }
}
