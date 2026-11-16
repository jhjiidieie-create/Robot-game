using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform puckPoint;

    Puck targetPuck;

    public void StartRobot()
    {
        targetPuck = null;
        float minDistance = Mathf.Infinity;

        foreach (Puck puck in FindObjectsOfType<Puck>())
        {
            if (puck.isSorted) continue;

            Transform point = puck.pickupPoint != null ? puck.pickupPoint : puck.transform;
            float distance = (point.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                targetPuck = puck;
            }
        }

        if (targetPuck == null) return;

        agent.enabled = true;
        agent.isStopped = false;
        agent.SetDestination(targetPuck.pickupPoint != null ? targetPuck.pickupPoint.position : targetPuck.transform.position);
    }

    void Update()
    {
        if (targetPuck == null || puckPoint == null || agent.pathPending) return;
        if (agent.remainingDistance > agent.stoppingDistance) return;

        agent.isStopped = true;

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

        targetPuck = null;
    }
}
