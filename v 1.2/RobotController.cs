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

        foreach (Puck puck in FindObjectsOfType<Puck>())
        {
            if (puck.isSorted) continue;

            float distance = (puck.transform.position - transform.position).sqrMagnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                targetPuck = puck;
            }
        }

        if (targetPuck == null) return;

        agent.isStopped = false;
        Transform point = targetPuck.pickupPoint != null ? targetPuck.pickupPoint : targetPuck.transform;
        agent.SetDestination(point.position);
    }

    void PickUpPuck()
    {
        targetPuck.transform.SetParent(puckPoint);
        targetPuck.transform.localPosition = Vector3.zero;
        targetPuck.transform.localRotation = Quaternion.identity;

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
            if (hole.isFilled || hole.dropPoint == null) continue;

            float distance = (hole.dropPoint.position - transform.position).sqrMagnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                targetHole = hole;
            }
        }

        if (targetHole == null) return;

        agent.isStopped = false;
        agent.SetDestination(targetHole.dropPoint.position);
    }

    void DropPuck()
    {
        targetPuck.transform.SetParent(targetHole.dropPoint);
        targetPuck.transform.localPosition = Vector3.zero;
        targetPuck.transform.localRotation = Quaternion.identity;
        targetPuck.isSorted = true;
        targetHole.isFilled = true;

        hasPuck = false;
        targetPuck = null;
        targetHole = null;

        FindNextPuck();
    }
}
