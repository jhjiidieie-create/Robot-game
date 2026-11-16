using UnityEngine;

public enum RobotPartType { Head, Body }

public class RobotPart : MonoBehaviour
{
    public RobotPartType partType = RobotPartType.Body;
    public RobotAttachmentSystem attachmentSystem;

    [HideInInspector] public RobotMountPoint currentMountPoint;

    public bool IsAttached => currentMountPoint != null;

    void Update()
    {
        if (!IsAttached && attachmentSystem != null)
            attachmentSystem.CheckForAutoAttach(this);
    }
}
