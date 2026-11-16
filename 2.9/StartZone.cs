using UnityEngine;
public class StartZone : MonoBehaviour
{
    public bool isOccupied;
    int inside;
    void OnTriggerEnter(Collider other) { inside++; isOccupied = true; }
    void OnTriggerExit(Collider other) { inside = Mathf.Max(0, inside - 1); isOccupied = inside > 0; }
}