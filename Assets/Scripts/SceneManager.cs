using UnityEngine;

public class SceneManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var saveManager = SaveManager.instance;
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            if (currentScene == "MainMenuScene")
            {
                // Load the last scene from GameData
                string lastScene = saveManager.gameData.defaultScene.sceneName;
                if (!string.IsNullOrEmpty(lastScene))
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(lastScene);
                    
                }
            }
            else
            {
                // Save and load MainMenuScene
                saveManager.UpdateGameData();
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
            }
        }
    }
}
