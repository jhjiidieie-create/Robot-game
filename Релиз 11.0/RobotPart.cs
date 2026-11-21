using UnityEngine;

public class RobotPart : MonoBehaviour
{
    public enum PartType { Motor, PowerSupply, Manipulator, Tracks }

    public PartType partType;
    public bool isAttached;
}