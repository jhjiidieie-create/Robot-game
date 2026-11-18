using UnityEngine;

public class ManualSlot : MonoBehaviour
{
    public int slotNumber;
    public GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        Puck puck = other.GetComponentInParent<Puck>();
        if (puck == null || puck.isSorted || puck.number != slotNumber) return;
        puck.isSorted = true;
        gameManager.AddPuck();
    }
}