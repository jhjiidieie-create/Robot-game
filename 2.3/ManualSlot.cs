using UnityEngine;

public class ManualSlot : MonoBehaviour
{
    public int slotNumber;
    public bool slotFilled;
    public ManualGameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        ManualPuck puck = other.GetComponentInParent<ManualPuck>();

        if (puck == null || puck.alreadySorted || slotFilled) return;
        if (puck.puckNumber != slotNumber || gameManager == null) return;

        puck.alreadySorted = true;
        slotFilled = true;
        gameManager.CheckWin();
    }
}
