using UnityEngine;

public class RobotAssembly : MonoBehaviour
{
    public RobotMountPoint motorMount, powerSupplyMount, manipulatorMount, tracksMount;
    public StartZone startZone;
    public RobotController automaticRobot;
    public ManualRobotController manualRobot;
    public GameManager gameManager;

    public void StartRobot()
    {
        if (motorMount == null || !motorMount.IsOccupied ||
            powerSupplyMount == null || !powerSupplyMount.IsOccupied ||
            manipulatorMount == null || !manipulatorMount.IsOccupied ||
            tracksMount == null || !tracksMount.IsOccupied ||
            startZone == null || !startZone.isOccupied) return;

        if (automaticRobot != null) automaticRobot.StartRobot();
        if (manualRobot != null)
        {
            manualRobot.StartRobot();
            if (gameManager != null) gameManager.StartGame();
        }
    }
}