using UnityEngine;

public class PuckSlot : MonoBehaviour
{
    public int number;
    public Transform dropPoint, approachPoint;
    public bool isFilled;
    public GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        Puck puck = other.GetComponentInParent<Puck>();
        if (gameManager == null || isFilled || puck == null || puck.isSorted || puck.number != number) return;
        isFilled = puck.isSorted = true;
        gameManager.AddPuck();
    }
}