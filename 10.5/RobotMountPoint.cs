using UnityEngine;
using Valve.VR.InteractionSystem;

public class RobotMountPoint : MonoBehaviour
{
    public RobotPart.PartType requiredType;
    public float attachDistance = 0.15f;
    public RobotPart installedPart;
    public bool IsOccupied => installedPart != null;

    RobotPart[] parts;

    void Start() => parts = FindObjectsOfType<RobotPart>();

    void Update()
    {
        if (installedPart != null) return;

        foreach (RobotPart part in parts)
        {
            if (part == null || part.isAttached || part.partType != requiredType ||
                (transform.position - part.transform.position).sqrMagnitude > attachDistance * attachDistance) continue;

            Interactable item = part.GetComponent<Interactable>();
            if (item == null || item.attachedToHand == null) continue;

            item.attachedToHand.DetachObject(part.gameObject, false);
            item.enabled = false;

            Throwable throwable = part.GetComponent<Throwable>();
            if (throwable != null) throwable.enabled = false;

            Rigidbody rb = part.GetComponent<Rigidbody>();
            if (rb != null) { rb.isKinematic = true; rb.useGravity = false; }
            foreach (Collider col in part.GetComponentsInChildren<Collider>()) col.enabled = false;

            part.isAttached = true;
            installedPart = part;
            part.transform.SetParent(transform);
            part.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            return;
        }
    }
}