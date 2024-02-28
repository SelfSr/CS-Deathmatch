using UnityEngine;
using UnityEngine.AI;

public class DebugNavMeshAgent : MonoBehaviour
{
    public bool velocity;
    public bool desireVelocity;
    public bool path;

    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void OnDrawGizmos()
    {
        if (agent != null)
        {
            if (velocity)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
            }
            if (desireVelocity)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);
            }
            if (path)
            {
                Gizmos.color = Color.black;
                var agentPath = agent.path;
                Vector3 prevCorner = transform.position;
                foreach (var corner in agentPath.corners)
                {
                    Gizmos.DrawLine(prevCorner, corner);
                    Gizmos.DrawSphere(corner, 0.1f);
                    prevCorner = corner;
                }
            }
        }
    }
}