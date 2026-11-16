using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform puckPoint;
    public GameManager gameManager;
    Puck puck;
    Hole hole;
    bool carrying;

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
        puck = null; hole = null; carrying = false;
    }

    void Update()
    {
        if ((puck == null && !carrying) || agent == null || !agent.enabled || agent.pathPending || agent.remainingDistance > agent.stoppingDistance) return;
        agent.isStopped = true;
        if (!carrying) PickUpPuck();
        else if (hole == null) FindNextHole();
        else DropPuck();
    }

    void FindNextPuck()
    {
        puck = null;
        float min = Mathf.Infinity;
        Hole[] holes = FindObjectsOfType<Hole>();
        foreach (Puck item in FindObjectsOfType<Puck>())
        {
            if (item.isSorted || !HasFreeHole(item.number, holes)) continue;
            float distance = (item.transform.position - transform.position).sqrMagnitude;
            if (distance >= min) continue;
            min = distance; puck = item;
        }
        if (puck == null) return;
        agent.isStopped = false;
        agent.SetDestination((puck.pickupPoint ? puck.pickupPoint : puck.transform).position);
    }

    bool HasFreeHole(int number, Hole[] holes)
    {
        foreach (Hole item in holes)
            if (!item.isFilled && item.number == number && item.dropPoint != null) return true;
        return false;
    }

    void PickUpPuck()
    {
        puck.transform.SetParent(puckPoint);
        puck.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        Rigidbody rb = puck.GetComponent<Rigidbody>();
        if (rb != null) { rb.isKinematic = true; rb.useGravity = false; }
        foreach (Collider col in puck.GetComponentsInChildren<Collider>()) col.enabled = false;
        carrying = true;
        FindNextHole();
    }

    void FindNextHole()
    {
        hole = null;
        float min = Mathf.Infinity;
        foreach (Hole item in FindObjectsOfType<Hole>())
        {
            if (item.isFilled || item.number != puck.number || item.dropPoint == null) continue;
            Transform point = item.approachPoint ? item.approachPoint : item.dropPoint;
            float distance = (point.position - transform.position).sqrMagnitude;
            if (distance >= min) continue;
            min = distance; hole = item;
        }
        if (hole == null) return;
        agent.isStopped = false;
        agent.SetDestination((hole.approachPoint ? hole.approachPoint : hole.dropPoint).position);
    }

    void DropPuck()
    {
        puck.transform.SetParent(hole.dropPoint);
        puck.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        puck.isSorted = true; hole.isFilled = true;
        carrying = false; puck = null; hole = null;
        if (gameManager == null || !gameManager.CheckWin()) FindNextPuck();
    }
}