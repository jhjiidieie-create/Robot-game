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

            // Получаем компоненты только устанавливаемой детали до её перемещения в робота.
            Collider[] partColliders = part.GetComponentsInChildren<Collider>(true);
            Throwable throwable = part.GetComponent<Throwable>();
            Rigidbody rb = part.GetComponent<Rigidbody>();

            item.attachedToHand.DetachObject(part.gameObject, false);
            item.enabled = false;
            if (throwable != null) throwable.enabled = false;
            if (rb != null) { rb.isKinematic = true; rb.useGravity = false; }

            foreach (Collider col in partColliders)
                if (col != null) col.enabled = false;

            part.isAttached = true;
            installedPart = part;
            part.transform.SetParent(transform);
            part.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            return;
        }
    }
}