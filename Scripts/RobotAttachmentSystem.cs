using UnityEngine;
using Valve.VR.InteractionSystem;

public class RobotAttachmentSystem : MonoBehaviour
{
    public RobotMountPoint[] mountPoints;

    public void CheckForAutoAttach(RobotPart part)
    {
        if (part == null || part.IsAttached)
            return;

        Interactable interactable = part.GetComponent<Interactable>();

        if (interactable == null || interactable.attachedToHand == null)
            return;

        foreach (RobotMountPoint point in mountPoints)
        {
            if (point == null || !point.CanAccept(part))
                continue;

            if ((part.transform.position - point.transform.position).sqrMagnitude <= point.attachDistance * point.attachDistance)
            {
                TryAttach(part, point, interactable);
                return;
            }
        }
    }

    void TryAttach(RobotPart part, RobotMountPoint point, Interactable interactable)
    {
        interactable.attachedToHand.DetachObject(part.gameObject, false);

        if (!point.TryReserve(part))
            return;

        interactable.enabled = false;

        Throwable throwable = part.GetComponent<Throwable>();
        if (throwable != null)
            throwable.enabled = false;

        Rigidbody rb = part.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.detectCollisions = false;
        }

        foreach (Collider col in part.GetComponentsInChildren<Collider>())
            col.enabled = false;

        part.transform.SetParent(point.transform);
        part.transform.localPosition = Vector3.zero;
        part.transform.localRotation = Quaternion.identity;
    }
}
