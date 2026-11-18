using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform puckPoint;
    public GameManager gameManager;

    Puck targetPuck;
    Hole targetHole;
    bool hasPuck;

    public void StartRobot()
    {
        if (agent == null) return;
        agent.enabled = true;
        if (gameManager != null) gameManager.StartGame();
        FindNextPuck();
    }

    public void StopRobot()
    {
        if (agent != null && agent.enabled) agent.isStopped = true;
        targetPuck = null;
        targetHole = null;
        hasPuck = false;
    }

    void Update()
    {
        if (targetPuck == null && !hasPuck) return;
        if (agent == null || !agent.enabled || agent.pathPending ||
            agent.remainingDistance > agent.stoppingDistance) return;

        agent.isStopped = true;
        if (!hasPuck) PickUpPuck();
        else if (targetHole == null) FindNextHole();
        else DropPuck();
    }

    void FindNextPuck()
    {
        targetPuck = null;
        float min = Mathf.Infinity;
        Puck[] pucks = FindObjectsOfType<Puck>();
        Hole[] holes = FindObjectsOfType<Hole>();

        foreach (Puck puck in pucks)
        {
            if (puck.isSorted || puck.isCarried) continue;
            bool hasHole = false;
            foreach (Hole hole in holes)
                if (!hole.isFilled && hole.number == puck.number && hole.dropPoint != null)
                { hasHole = true; break; }
            if (!hasHole) continue;

            float d = (puck.transform.position - transform.position).sqrMagnitude;
            if (d >= min) continue;
            min = d;
            targetPuck = puck;
        }

        if (targetPuck == null) return;
        agent.isStopped = false;
        agent.SetDestination((targetPuck.pickupPoint != null ? targetPuck.pickupPoint : targetPuck.transform).position);
    }

    void PickUpPuck()
    {
        targetPuck.transform.SetParent(puckPoint);
        targetPuck.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        Rigidbody rb = targetPuck.GetComponent<Rigidbody>();
        if (rb != null) { rb.isKinematic = true; rb.useGravity = false; }
        foreach (Collider col in targetPuck.GetComponentsInChildren<Collider>()) col.enabled = false;

        hasPuck = true;
        FindNextHole();
    }

    void FindNextHole()
    {
        targetHole = null;
        float min = Mathf.Infinity;
        foreach (Hole hole in FindObjectsOfType<Hole>())
        {
            if (hole.isFilled || hole.number != targetPuck.number || hole.dropPoint == null) continue;
            Transform point = hole.approachPoint != null ? hole.approachPoint : hole.dropPoint;
            float d = (point.position - transform.position).sqrMagnitude;
            if (d >= min) continue;
            min = d;
            targetHole = hole;
        }

        if (targetHole == null) return;
        agent.isStopped = false;
        agent.SetDestination((targetHole.approachPoint != null ? targetHole.approachPoint : targetHole.dropPoint).position);
    }

    void DropPuck()
    {
        targetPuck.transform.SetParent(targetHole.dropPoint);
        targetPuck.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        targetPuck.isSorted = targetHole.isFilled = true;
        hasPuck = false;
        targetPuck = null;
        targetHole = null;

        if (!gameManager.AddPuck()) FindNextPuck();
    }
}