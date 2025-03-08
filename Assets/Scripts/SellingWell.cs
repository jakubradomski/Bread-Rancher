using UnityEngine;

public class SellingWell : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var bread = other.GetComponent<BreadController>();

        if(bread != null)
        {
            CoinManager.Instance.Coins++;
            bread.GrindToFibrousPowder();
        }
    }
}
