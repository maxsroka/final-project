
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneObjectDisabler : MonoBehaviour
{
    [SerializeField] string sceneName;
    
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            gameObject.SetActive(false);            
        }
    }
}
