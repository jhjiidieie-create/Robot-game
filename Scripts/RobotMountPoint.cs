using UnityEngine;

public class RobotMountPoint : MonoBehaviour
{
    public RobotPartType acceptedPartType = RobotPartType.Body;
    public float attachDistance = 0.15f;

    [SerializeField] RobotPart installedPart;

    public bool IsOccupied => installedPart != null;

    public bool CanAccept(RobotPart part)
    {
        return part != null && !IsOccupied && !part.IsAttached && part.partType == acceptedPartType;
    }

    public bool TryReserve(RobotPart part)
    {
        if (!CanAccept(part))
            return false;

        installedPart = part;
        part.currentMountPoint = this;
        return true;
    }

    public void ReleasePart()
    {
        if (installedPart == null)
            return;

        installedPart.currentMountPoint = null;
        installedPart = null;
    }
}
