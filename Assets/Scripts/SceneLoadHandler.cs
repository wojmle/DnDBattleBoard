using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadHandler : MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log("SceneLoadHandler enabled");
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        Debug.Log("SceneLoadHandler disabled");
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SceneLoaded");
        // Only load data for gameplay scenes, not for MainMenuScene
        if (scene.name != "MainMenuScene" && SaveManager.instance != null)
        {
            var buildingManager = FindObjectOfType<BuildingManager>();
            if (buildingManager != null)
            {
                buildingManager.LoadData(SaveManager.instance.gameData);
            }
        }
    }
}