using UnityEngine;

public class RobotAssembly : MonoBehaviour
{
    public RobotMountPoint motorMount;
    public RobotMountPoint powerSupplyMount;
    public RobotMountPoint manipulatorMount;
    public RobotMountPoint tracksMount;
    public RobotMountPoint buttonMount;

    public StartZone startZone;
    public RobotController automaticRobot;
    public ManualRobotController manualRobot;
    public GameManager gameManager;

    public void StartRobot()
    {
        if (motorMount == null || powerSupplyMount == null || manipulatorMount == null ||
            tracksMount == null || buttonMount == null || startZone == null ||
            !motorMount.IsOccupied || !powerSupplyMount.IsOccupied ||
            !manipulatorMount.IsOccupied || !tracksMount.IsOccupied ||
            !buttonMount.IsOccupied || !startZone.isOccupied) return;

        if (automaticRobot != null) automaticRobot.StartRobot();
        if (manualRobot == null) return;

        manualRobot.StartRobot();
        if (gameManager != null) gameManager.StartGame();
    }
}