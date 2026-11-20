using UnityEngine;

public class RobotAssembly : MonoBehaviour
{
    public RobotMountPoint headMount;
    public StartZone startZone;
    public RobotController automaticRobot;
    public ManualRobotController manualRobot;
    public GameManager gameManager;

    public void StartRobot()
    {
        if (!headMount.IsOccupied || !startZone.isOccupied) return;
        if (automaticRobot != null) automaticRobot.StartRobot();
        if (manualRobot == null) return;
        manualRobot.StartRobot();
        gameManager.StartGame();
    }
}