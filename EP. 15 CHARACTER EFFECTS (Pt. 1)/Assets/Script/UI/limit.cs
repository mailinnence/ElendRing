using UnityEngine;
using UnityEngine.SceneManagement;

public class limit : MonoBehaviour
{
    [SerializeField] private GameObject targetObject; // 활성화할 오브젝트

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Scene_World_01")
        {
            targetObject.SetActive(true);
        }
        else
        {
            targetObject.SetActive(false);
        }
    }
}
