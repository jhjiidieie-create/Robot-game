using UnityEngine;

public class ManualRobotAssembly : MonoBehaviour
{
    public RobotMountPoint headMount;
    public ManualRobotController robotController;
    public StartZone startZone;
    public GameManager gameManager;

    public void StartRobot()
    {
        if (!headMount.IsOccupied || !startZone.isOccupied) return;
        robotController.StartRobot();
        gameManager.StartGame();
    }
}