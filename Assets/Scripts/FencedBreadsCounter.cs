using System.Collections.Generic;
using UnityEngine;

public class FencedBreadsCounter : MonoBehaviour
{
    public int Counter = 0;
    private HashSet<GameObject> countedBreads = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        var bread = other.GetComponent<BreadController>();
        if (bread != null && !countedBreads.Contains(bread.gameObject))
        {
            countedBreads.Add(bread.gameObject);
            Counter++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var bread = other.GetComponent<BreadController>();
        if (bread != null && countedBreads.Contains(bread.gameObject))
        {
            countedBreads.Remove(bread.gameObject);
            Counter--;
        }
    }
}
