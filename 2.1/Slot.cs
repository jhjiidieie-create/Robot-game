using UnityEngine;

public class Slot : MonoBehaviour
{
    public int number;
    public bool isFilled;
    public GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        Puck puck = other.GetComponent<Puck>();

        if (puck == null || puck.isSorted || isFilled) return;
        if (puck.number != number) return;

        puck.isSorted = true;
        isFilled = true;

        if (gameManager != null)
            gameManager.AddPuck();
    }
}