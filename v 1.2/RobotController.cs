using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public NavMeshAgent agent;

    public void StartRobot()
    {
        Puck nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Puck puck in FindObjectsOfType<Puck>())
        {
            if (puck.isSorted) continue;

            float distance = (puck.transform.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = puck;
            }
        }

        if (nearest == null) return;

        agent.enabled = true;
        agent.SetDestination(nearest.transform.position);
    }
}
