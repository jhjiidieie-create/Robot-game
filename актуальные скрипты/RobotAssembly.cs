using UnityEngine;

public class RobotAssembly : MonoBehaviour
{
    public RobotMountPoint headMount;
    public RobotController robotController;
    public StartZone startZone;

    public void StartRobot()
    {
        if (headMount != null && headMount.installedPart != null &&
            robotController != null && startZone != null && startZone.isOccupied)
            robotController.StartRobot();
    }
}