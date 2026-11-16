using UnityEngine;
using UnityEngine.AI;

public class RobotAssembly : MonoBehaviour
{
    public RobotMountPoint headMount;
    public NavMeshAgent agent;

    public void StartRobot()
    {
        if (headMount == null || headMount.installedPart == null) return;

        agent.enabled = true;
    }
}
