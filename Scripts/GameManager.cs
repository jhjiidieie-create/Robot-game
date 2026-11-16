using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public NavMeshAgent robotAgent;
    public RobotMountPoint headMountPoint, bodyMountPoint;
    public Transform robotStartPoint;
    public float allowedDistance = 1f;

    bool robotStarted;

    void Update()
    {
        if (robotStarted || !headMountPoint.IsOccupied || !bodyMountPoint.IsOccupied)
            return;

        if ((robotAgent.transform.position - robotStartPoint.position).sqrMagnitude > allowedDistance * allowedDistance)
            return;

        robotAgent.enabled = true;
        robotStarted = true;
    }
}
