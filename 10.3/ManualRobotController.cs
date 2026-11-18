using UnityEngine;
using Valve.VR;

public class ManualRobotController : MonoBehaviour
{
    public Transform carryPoint;
    public Puck[] pucks;
    public float moveSpeed = 2f, turnSpeed = 90f, grabDistance = 1f;

    Puck carriedPuck;
    Rigidbody puckRb;
    Collider[] puckColliders;
    bool active;

    public bool IsMoving { get; private set; }

    public void StartRobot() => active = true;
    public void StopRobot() { active = IsMoving = false; }

    void Update()
    {
        if (!active) return;

        var hand = SteamVR_Input_Sources.LeftHand;
        bool f = SteamVR_Actions.default_Forward.GetState(hand);
        bool b = SteamVR_Actions.default_Backward.GetState(hand);
        bool l = SteamVR_Actions.default_Left.GetState(hand);
        bool r = SteamVR_Actions.default_Right.GetState(hand);

        transform.Translate(Vector3.forward * ((f ? 1 : 0) - (b ? 1 : 0)) * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, ((r ? 1 : 0) - (l ? 1 : 0)) * turnSpeed * Time.deltaTime);
        IsMoving = f || b || l || r;

        if (SteamVR_Actions.default_Magnet.GetStateDown(hand)) PickUpPuck();
        if (SteamVR_Actions.default_Release.GetStateDown(hand)) ReleasePuck();
    }

    void PickUpPuck()
    {
        if (carriedPuck != null) return;

        float min = grabDistance;
        foreach (Puck puck in pucks)
        {
            if (puck == null || puck.isSorted || puck.isCarried) continue;
            float d = (carryPoint.position - puck.transform.position).sqrMagnitude;
            if (d >= min * min) continue;
            min = Mathf.Sqrt(d);
            carriedPuck = puck;
        }

        if (carriedPuck == null) return;
        carriedPuck.isCarried = true;
        puckRb = carriedPuck.GetComponentInChildren<Rigidbody>();
        puckColliders = carriedPuck.GetComponentsInChildren<Collider>();
        puckRb.isKinematic = true;
        puckRb.useGravity = false;
        foreach (Collider col in puckColliders) col.enabled = false;
        carriedPuck.transform.SetParent(carryPoint);
        carriedPuck.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    void ReleasePuck()
    {
        if (carriedPuck == null) return;
        carriedPuck.transform.SetParent(null);
        carriedPuck.isCarried = false;
        puckRb.isKinematic = false;
        puckRb.useGravity = true;
        foreach (Collider col in puckColliders) col.enabled = true;
        carriedPuck = null;
    }
}