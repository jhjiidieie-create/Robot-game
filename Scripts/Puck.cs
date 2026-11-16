using UnityEngine;

public class Puck : MonoBehaviour
{
    [Header("Puck setting")]
    public int puckNumber;

    [HideInInspector] public bool isSorted;
    [HideInInspector] public bool isCarried;
}
