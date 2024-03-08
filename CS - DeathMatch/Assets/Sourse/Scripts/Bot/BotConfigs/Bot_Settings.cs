using UnityEngine;

[CreateAssetMenu(fileName = "BotConfig", menuName = "Bot/New BotConfig")]
public class Bot_Settings : ScriptableObject
{

    [Header("MOVING")]
    [SerializeField] private float _searchRadius;
    [SerializeField] private float _botSpeed;

    public float searchRadius => _searchRadius;
    public float botSpeed => _botSpeed;

    [Header("FIGHT TACTICS")]
    [SerializeField] private float _stoppingDistance = 7;

    public float stoppingDistance => _stoppingDistance;

}
