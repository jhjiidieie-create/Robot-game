using UnityEngine;
public class StartZone : MonoBehaviour
{
    public bool isOccupied;
    int inside;
    void OnTriggerEnter(Collider other) { inside++; isOccupied = true; }
    void OnTriggerExit(Collider other) { isOccupied = --inside > 0; }
}