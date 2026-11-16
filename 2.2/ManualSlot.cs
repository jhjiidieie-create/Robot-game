using UnityEngine;

public class ManualSlot : MonoBehaviour
{
    public int slotNumber;
    public bool slotOccupied;
    public ManualGameManager manualManager;

    void OnTriggerEnter(Collider other)
    {
        ManualPuck foundPuck = other.GetComponent<ManualPuck>();

        if (foundPuck == null || foundPuck.alreadySorted || slotOccupied) return;
        if (foundPuck.puckNumber != slotNumber || manualManager == null) return;
        if (!manualManager.AddPuck()) return;

        foundPuck.alreadySorted = true;
        slotOccupied = true;
    }
}
