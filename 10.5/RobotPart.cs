using UnityEngine;

public class RobotPart : MonoBehaviour
{
    public enum PartType { Motor, PowerSupply, Manipulator, Tracks, StartButton }

    public PartType partType;
    public bool isAttached;
}