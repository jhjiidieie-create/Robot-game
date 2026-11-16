using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform puckPoint;

    Puck targetPuck;
    Hole targetHole;
    bool hasPuck;

    public void StartRobot()
    {
        agent.enabled = true;
        FindNextPuck();
    }

    void Update()
    {
        if (targetPuck == null && !hasPuck) return;
        if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance) return;

        agent.isStopped = true;

        if (!hasPuck)
        {
            PickUpPuck();
            return;
        }

        if (targetHole == null)
        {
            FindNextHole();
            return;
        }

        DropPuck();
    }

    void FindNextPuck()
    {
        targetPuck = null;
        float minDistance = Mathf.Infinity;
        Hole[] holes = FindObjectsOfType<Hole>();

        foreach (Puck puck in FindObjectsOfType<Puck>())
        {
            if (puck.isSorted) continue;

            bool hasHole = false;
            foreach (Hole hole in holes)
            {
                if (!hole.isFilled && hole.number == puck.number && hole.dropPoint != null)
                {
                    hasHole = true;
                    break;
                }
            }

            if (!hasHole) continue;

            float distance = (puck.transform.position - transform.position).sqrMagnitude;
            if (distance >= minDistance) continue;

            minDistance = distance;
            targetPuck = puck;
        }

        if (targetPuck == null) return;

        agent.isStopped = false;
        agent.SetDestination((targetPuck.pickupPoint ?? targetPuck.transform).position);
    }

    void PickUpPuck()
    {
        targetPuck.transform.SetParent(puckPoint);
        targetPuck.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        Rigidbody rb = targetPuck.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        foreach (Collider col in targetPuck.GetComponentsInChildren<Collider>())
            col.enabled = false;

        hasPuck = true;
        FindNextHole();
    }

    void FindNextHole()
    {
        targetHole = null;
        float minDistance = Mathf.Infinity;

        foreach (Hole hole in FindObjectsOfType<Hole>())
        {
            if (hole.isFilled || hole.dropPoint == null || hole.number != targetPuck.number) continue;

            Transform point = hole.approachPoint ?? hole.dropPoint;
            float distance = (point.position - transform.position).sqrMagnitude;
            if (distance >= minDistance) continue;

            minDistance = distance;
            targetHole = hole;
        }

        if (targetHole == null) return;

        agent.isStopped = false;
        agent.SetDestination((targetHole.approachPoint ?? targetHole.dropPoint).position);
    }

    void DropPuck()
    {
        targetPuck.transform.SetParent(targetHole.dropPoint);
        targetPuck.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        targetPuck.isSorted = true;
        targetHole.isFilled = true;

        hasPuck = false;
        targetPuck = null;
        targetHole = null;
        FindNextPuck();
    }
}
