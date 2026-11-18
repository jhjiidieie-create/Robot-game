using UnityEngine;

public class ManualSlot : MonoBehaviour
{
    public int slotNumber;
    public GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        ManualPuck puck = other.GetComponentInParent<ManualPuck>();
        if (puck == null || puck.alreadySorted || puck.puckNumber != slotNumber) return;
        puck.alreadySorted = true;
        gameManager.AddPuck();
    }
}