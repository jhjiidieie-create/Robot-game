using UnityEngine;

public class StartZone : MonoBehaviour
{
    public bool isOccupied;
    int objectsInside;

    void OnTriggerEnter(Collider other)
    {
        isOccupied = ++objectsInside > 0;
    }

    void OnTriggerExit(Collider other)
    {
        objectsInside = Mathf.Max(0, objectsInside - 1);
        isOccupied = objectsInside > 0;
    }
}