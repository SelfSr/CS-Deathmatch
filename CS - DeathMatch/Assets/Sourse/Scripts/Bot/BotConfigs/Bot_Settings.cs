using UnityEngine;

[CreateAssetMenu(fileName = "BotConfig", menuName = "Bot/New BotConfig")]
public class Bot_Settings : ScriptableObject
{
    [Header("MAIN PARAMETERS")]

    [Header("VIEW")]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private int _targetCount;
    [SerializeField] private float _timeCheckVisiableTarget;

    public LayerMask layerMask => _layerMask;
    public int targetCount => _targetCount;
    public float timeCheckVisiableTarget => _timeCheckVisiableTarget;

    [Header("MOVING")]
    [SerializeField] private float _searchRadius;
    [SerializeField] private float _botSpeed;

    public float searchRadius => _searchRadius;
    public float botSpeed => _botSpeed;
}
