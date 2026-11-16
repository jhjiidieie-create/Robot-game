using UnityEngine;

public class ManualRobotAssembly : MonoBehaviour
{
    public RobotMountPoint headMount;
    public ManualRobotController robotController;
    public StartZone startZone;
    public ManualGameManager gameManager;

    public void StartRobot()
    {
        if (headMount != null && headMount.IsOccupied &&
            robotController != null && startZone != null && startZone.isOccupied)
        {
            robotController.StartRobot();
            gameManager.StartGame();
        }
    }
}
