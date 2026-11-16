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

    public void StopRobot()
    {
        active = false;
        IsMoving = false;
    }

    void Update()
    {
        if (!active) return;

        bool forward = SteamVR_Actions.default_Forward.GetState(SteamVR_Input_Sources.LeftHand);
        bool backward = SteamVR_Actions.default_Backward.GetState(SteamVR_Input_Sources.LeftHand);
        bool left = SteamVR_Actions.default_Left.GetState(SteamVR_Input_Sources.LeftHand);
        bool right = SteamVR_Actions.default_Right.GetState(SteamVR_Input_Sources.LeftHand);

        transform.Translate(Vector3.forward * ((forward ? 1 : 0) - (backward ? 1 : 0)) * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, ((right ? 1 : 0) - (left ? 1 : 0)) * turnSpeed * Time.deltaTime);
        IsMoving = forward || backward || left || right;

        if (SteamVR_Actions.default_Magnet.GetStateDown(SteamVR_Input_Sources.LeftHand)) PickUpPuck();
        if (SteamVR_Actions.default_Release.GetStateDown(SteamVR_Input_Sources.LeftHand)) ReleasePuck();
    }

    void PickUpPuck()
    {
        if (carriedPuck != null) return;

        float minDistance = grabDistance;
        foreach (ManualPuck puck in pucks)
        {
            if (puck == null || puck.alreadySorted || puck.isCarried) continue;

            float distance = Vector3.Distance(transform.position, puck.transform.position);
            if (distance >= minDistance) continue;

            minDistance = distance;
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