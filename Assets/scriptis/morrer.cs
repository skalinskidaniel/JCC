using UnityEngine;
using UnityEngine.SceneManagement;

public class morrer : MonoBehaviour
{
    public string proximafase = "fase02";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("morre"))
        {
            SceneManager.LoadScene(proximafase);
        }
    }
}
