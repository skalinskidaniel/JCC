using UnityEngine;
using UnityEngine.SceneManagement;

public class ganhar : MonoBehaviour


{
    public string proximafase = "fase2";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ganhar"))
        {
            SceneManager.LoadScene(proximafase);
        }
        
         if (other.CompareTag("morre"))
        {
            SceneManager.LoadScene(proximafase);
        }
    }
}
