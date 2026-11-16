using UnityEngine;
using Valve.VR.InteractionSystem;

public class RobotMountPoint : MonoBehaviour
{
    public float attachDistance = 0.15f;
    public RobotPart installedPart;
    public bool IsOccupied => installedPart != null;
    RobotPart part;

    void Update()
    {
        if (installedPart != null) return;
        if (part == null) part = FindObjectOfType<RobotPart>();
        if (part == null || part.isAttached) return;
        Interactable interactable = part.GetComponent<Interactable>();
        if (interactable == null || interactable.attachedToHand == null) return;
        if ((transform.position - part.transform.position).sqrMagnitude > attachDistance * attachDistance) return;
        interactable.attachedToHand.DetachObject(part.gameObject, false);
        interactable.enabled = false;
        Throwable throwable = part.GetComponent<Throwable>();
        if (throwable != null) throwable.enabled = false;
        Rigidbody rb = part.GetComponent<Rigidbody>();
        if (rb != null) { rb.isKinematic = true; rb.useGravity = false; }
        foreach (Collider col in part.GetComponentsInChildren<Collider>()) col.enabled = false;
        part.isAttached = true;
        installedPart = part;
        part.transform.SetParent(transform);
        part.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}