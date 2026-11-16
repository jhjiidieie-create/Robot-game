using UnityEngine;

public class RobotAssembly : MonoBehaviour
{
    public RobotMountPoint headMount;
    public RobotController robotController;

    public void StartRobot()
    {
        if (headMount != null && headMount.installedPart != null && robotController != null)
            robotController.StartRobot();
    }
}