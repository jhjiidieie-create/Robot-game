using UnityEngine;

public class RobotPart : MonoBehaviour
{
    public enum PartType { Head, Motor, PowerSupply, Manipulator, Tracks, StartButton }

    public PartType partType;
    public bool isAttached;
}