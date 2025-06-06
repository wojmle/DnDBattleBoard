using System.IO;
using Assets.Scripts;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private string saveFileName = "gameData.json";
    public static SaveManager instance { get; private set; }

    public GameData gameData { get; private set; }

    private FileDataHandler fileDataHandler;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            Debug.LogError("Found more than one SaveManager instance in the scene.");
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, "gameData.json");
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }
    
    public void SaveGame()
    {
        UpdateGameData();
        fileDataHandler.Save(gameData);
    }

    public void LoadGame(string loadData)
    {
        this.gameData = this.fileDataHandler.Load();
        
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }
    }

    public void UpdateGameData()
    {
        if (gameData == null)
        {
            Debug.LogWarning("gameData is null in UpdateGameData");
        }
        
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Gather scene data (implement your own data gathering logic)
        SceneData sceneData = new SceneData
        {
            sceneName = currentSceneName,
            // Fill in allies, enemies, terrain, etc.
        };

        // Gather Allies
        var allyControllers = FindObjectsOfType<AllyController>();
        foreach (var allyCtrl in allyControllers)
        {
            sceneData.allies.Add(new SceneAlly(allyCtrl.allyObject, allyCtrl.transform));
        }

        // Gather Enemies
        var adversaryControllers = FindObjectsOfType<EnemyController>();
        foreach (var advCtrl in adversaryControllers)
        {
            sceneData.enemies.Add(new SceneAdversary(advCtrl.adversary, advCtrl.transform));
        }

        if (currentSceneName == "DefaultScene")
        {
            gameData.defaultScene = sceneData;
        }
        else
        {
            int idx = gameData.scenes.FindIndex(s => s.sceneName == currentSceneName);
            if (idx >= 0)
                gameData.scenes[idx] = sceneData;
            else
                gameData.scenes.Add(sceneData);
        }
    }

    private void OnApplicationQuit()
    {
        //SaveGame();
    }
}
