using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBread : MonoBehaviour
{
    
    void Update()
    {
        transform.Rotate(new Vector3(1.5f * Time.deltaTime, 1 * Time.deltaTime, 1.75f * Time.deltaTime) * 25);

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject == gameObject)
                {
                    SceneManager.LoadScene(1);
                }
            }
        }
    }    
}
