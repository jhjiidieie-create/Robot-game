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
        if (agent != null && agent.enabled)
            agent.isStopped = true;

        targetPuck = null;
        targetHole = null;
        hasPuck = false;
    }

    void Update()
    {
        if (targetPuck == null && !hasPuck) return;
        if (agent == null || !agent.enabled || agent.pathPending || agent.remainingDistance > agent.stoppingDistance) return;

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
            if (puck.isSorted || !HasFreeHole(puck.number, holes)) continue;

            float distance = (puck.transform.position - transform.position).sqrMagnitude;
            if (distance >= minDistance) continue;

            minDistance = distance;
            targetPuck = puck;
        }

        if (targetPuck == null) return;

        agent.isStopped = false;
        agent.SetDestination((targetPuck.pickupPoint != null ? targetPuck.pickupPoint : targetPuck.transform).position);
    }

    bool HasFreeHole(int number, Hole[] holes)
    {
        foreach (Hole hole in holes)
            if (!hole.isFilled && hole.number == number && hole.dropPoint != null)
                return true;

        return false;
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
            if (hole.isFilled || hole.number != targetPuck.number || hole.dropPoint == null) continue;

            Transform point = hole.approachPoint != null ? hole.approachPoint : hole.dropPoint;
            float distance = (point.position - transform.position).sqrMagnitude;
            if (distance >= minDistance) continue;

            minDistance = distance;
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
        targetPuck.isSorted = true;
        targetHole.isFilled = true;

        hasPuck = false;
        targetPuck = null;
        targetHole = null;

        if (gameManager != null && gameManager.CheckWin()) return;
        FindNextPuck();
    }
}