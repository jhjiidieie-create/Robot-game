using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform puckPoint;
    public GameManager gameManager;

    Puck[] pucks;
    Puck targetPuck;
    PuckSlot[] slots;
    PuckSlot targetSlot;
    bool hasPuck;

    public void StartRobot()
    {
        if (agent == null) return;
        agent.enabled = true;
        if (gameManager != null) gameManager.StartGame();
        pucks = FindObjectsOfType<Puck>();
        slots = FindObjectsOfType<PuckSlot>();
        FindNextPuck();
    }

    public void StopRobot()
    {
        if (agent != null && agent.enabled) agent.isStopped = true;
        targetPuck = null;
        targetSlot = null;
        hasPuck = false;
    }

    void Update()
    {
        if ((targetPuck == null && !hasPuck) || agent == null || !agent.enabled ||
            agent.pathPending || agent.remainingDistance > agent.stoppingDistance) return;

        agent.isStopped = true;
        if (!hasPuck) PickUpPuck();
        else if (targetSlot == null) FindNextSlot();
        else DropPuck();
    }

    void FindNextPuck()
    {
        targetPuck = null;
        float min = Mathf.Infinity;

        foreach (Puck puck in pucks)
        {
            if (puck.isSorted || puck.isCarried || !HasFreeSlot(puck.number)) continue;
            float d = (puck.transform.position - transform.position).sqrMagnitude;
            if (d >= min) continue;
            min = d;
            targetPuck = puck;
        }

        if (targetPuck == null) return;
        agent.isStopped = false;
        agent.SetDestination((targetPuck.pickupPoint != null ? targetPuck.pickupPoint : targetPuck.transform).position);
    }

    bool HasFreeSlot(int number)
    {
        foreach (PuckSlot slot in slots)
            if (!slot.isFilled && slot.number == number && slot.dropPoint != null) return true;
        return false;
    }

    void PickUpPuck()
    {
        targetPuck.transform.SetParent(puckPoint);
        targetPuck.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        Rigidbody rb = targetPuck.GetComponent<Rigidbody>();
        if (rb != null) { rb.isKinematic = true; rb.useGravity = false; }
        foreach (Collider col in targetPuck.GetComponentsInChildren<Collider>()) col.enabled = false;
        hasPuck = true;
        FindNextSlot();
    }

    void FindNextSlot()
    {
        targetSlot = null;
        float min = Mathf.Infinity;
        foreach (PuckSlot slot in slots)
        {
            if (slot.isFilled || slot.number != targetPuck.number || slot.dropPoint == null) continue;
            Transform point = slot.approachPoint != null ? slot.approachPoint : slot.dropPoint;
            float d = (point.position - transform.position).sqrMagnitude;
            if (d >= min) continue;
            min = d;
            targetSlot = slot;
        }

        if (targetSlot == null) return;
        agent.isStopped = false;
        agent.SetDestination((targetSlot.approachPoint != null ? targetSlot.approachPoint : targetSlot.dropPoint).position);
    }

    void DropPuck()
    {
        targetPuck.transform.SetParent(targetSlot.dropPoint);
        targetPuck.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        targetPuck.isSorted = targetSlot.isFilled = true;
        hasPuck = false;
        targetPuck = null;
        targetSlot = null;
        if (!gameManager.AddPuck()) FindNextPuck();
    }
}