using UnityEngine;
using Valve.VR;

public class ManualRobotController : MonoBehaviour
{
    public Transform carryPoint;
    public ManualPuck[] pucks;
    public float moveSpeed = 2f, turnSpeed = 90f, grabDistance = 1f;
    ManualPuck carriedPuck;
    Rigidbody puckRb;
    Collider[] puckColliders;
    bool active;
    public bool IsMoving { get; private set; }

    public void StartRobot() => active = true;
    public void StopRobot() { active = false; IsMoving = false; }

    void Update()
    {
        if (!active) return;
        bool f = SteamVR_Actions.default_Forward.GetState(SteamVR_Input_Sources.LeftHand);
        bool b = SteamVR_Actions.default_Backward.GetState(SteamVR_Input_Sources.LeftHand);
        bool l = SteamVR_Actions.default_Left.GetState(SteamVR_Input_Sources.LeftHand);
        bool r = SteamVR_Actions.default_Right.GetState(SteamVR_Input_Sources.LeftHand);
        transform.Translate(Vector3.forward * ((f ? 1 : 0) - (b ? 1 : 0)) * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, ((r ? 1 : 0) - (l ? 1 : 0)) * turnSpeed * Time.deltaTime);
        IsMoving = f || b || l || r;
        if (SteamVR_Actions.default_Magnet.GetStateDown(SteamVR_Input_Sources.LeftHand)) PickUpPuck();
        if (SteamVR_Actions.default_Release.GetStateDown(SteamVR_Input_Sources.LeftHand)) ReleasePuck();
    }

    void PickUpPuck()
    {
        if (carriedPuck != null) return;
        float min = grabDistance * grabDistance;
        foreach (ManualPuck puck in pucks)
        {
            if (puck == null || puck.alreadySorted || puck.isCarried) continue;
            float distance = (transform.position - puck.transform.position).sqrMagnitude;
            if (distance >= min) continue;
            min = distance;
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
        carriedPuck.transform.localPosition = Vector3.zero;
        carriedPuck.transform.localRotation = Quaternion.identity;
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