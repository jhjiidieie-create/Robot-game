using UnityEngine;
using Valve.VR;

public class ManualRobotController : MonoBehaviour
{
    public Transform carryPoint;
    public ManualPuck[] pucks;
    public float moveSpeed = 2f, turnSpeed = 90f, grabDistance = 1f;
    ManualPuck carried;
    Rigidbody puckRb;
    Collider[] puckColliders;
    bool active;
    public bool IsMoving { get; private set; }

    public void StartRobot() => active = true;
    public void StopRobot() { active = false; IsMoving = false; }

    void Update()
    {
        if (!active) return;
        var hand = SteamVR_Input_Sources.LeftHand;
        bool f = SteamVR_Actions.default_Forward.GetState(hand), b = SteamVR_Actions.default_Backward.GetState(hand);
        bool l = SteamVR_Actions.default_Left.GetState(hand), r = SteamVR_Actions.default_Right.GetState(hand);
        float move = (f ? 1 : 0) - (b ? 1 : 0), turn = (r ? 1 : 0) - (l ? 1 : 0);
        transform.Translate(Vector3.forward * move * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, turn * turnSpeed * Time.deltaTime);
        IsMoving = move != 0 || turn != 0;
        if (SteamVR_Actions.default_Magnet.GetStateDown(hand)) PickUpPuck();
        if (SteamVR_Actions.default_Release.GetStateDown(hand)) ReleasePuck();
    }

    void PickUpPuck()
    {
        if (carried != null) return;
        float min = grabDistance * grabDistance;
        foreach (ManualPuck puck in pucks)
        {
            if (puck == null || puck.alreadySorted || puck.isCarried) continue;
            float distance = (transform.position - puck.transform.position).sqrMagnitude;
            if (distance >= min) continue;
            min = distance;
            carried = puck;
        }
        if (carried == null) return;
        carried.isCarried = true;
        puckRb = carried.GetComponentInChildren<Rigidbody>();
        puckColliders = carried.GetComponentsInChildren<Collider>();
        puckRb.isKinematic = true;
        puckRb.useGravity = false;
        foreach (Collider col in puckColliders) col.enabled = false;
        carried.transform.SetParent(carryPoint);
        carried.transform.localPosition = Vector3.zero;
        carried.transform.localRotation = Quaternion.identity;
    }

    void ReleasePuck()
    {
        if (carried == null) return;
        carried.transform.SetParent(null);
        carried.isCarried = false;
        puckRb.isKinematic = false;
        puckRb.useGravity = true;
        foreach (Collider col in puckColliders) col.enabled = true;
        carried = null;
    }
}