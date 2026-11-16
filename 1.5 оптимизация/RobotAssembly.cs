using UnityEngine;

public class RobotAssembly : MonoBehaviour
{
    public RobotMountPoint headMount;
    public RobotController robotController;

    public void StartRobot()
    {
        if (headMount == null || headMount.installedPart == null) return;
        robotController.StartRobot();
    }
}
