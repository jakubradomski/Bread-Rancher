using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableObject : MonoBehaviour
{
    public string Name;
    public HeldDistance HeldDistance = HeldDistance.normal;
}

public enum HeldDistance
{
    close = 5, 
    normal = 10,
    far = 12
}
