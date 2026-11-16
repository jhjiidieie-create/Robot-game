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

            Transform point = puck.pickupPoint != null ? puck.pickupPoint : puck.transform;
            float distance = (point.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = puck;
            }
        }

        if (nearest == null) return;

        Transform target = nearest.pickupPoint != null ? nearest.pickupPoint : nearest.transform;

        agent.enabled = true;
        agent.SetDestination(target.position);
    }
}
