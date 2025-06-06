using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainMenuButtons;
    public GameObject SaveWindow;
    public GameObject LoadWindow;
    public GameObject SaveWindowButton;
    public TMPro.TMP_InputField SaveWindowText;
    public TMPro.TMP_Dropdown LoadWindowDropdown;

    // Called by StartNewGame button
    public void OnStartNewGame()
    {
        Debug.Log("Starting a new game...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("DefaultScene");
        var saveManager = SaveManager.instance;
        saveManager.NewGame();
    }

    // Called by SaveGame button
    public void OnSaveWindowButton()
    {
        MainMenuButtons.SetActive(false);
        SaveWindow.SetActive(true);
    }

    // Called by LoadGame button
    public void OnLoadWindowButton()
    {
        MainMenuButtons.SetActive(false);
        LoadWindow.SetActive(true);

        // Get all save files in persistentDataPath
        string[] files = System.IO.Directory.GetFiles(Application.persistentDataPath, "*.json");
        LoadWindowDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        foreach (var file in files)
        {
            // Get file name without path and extension
            string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
            options.Add(fileName);
        }

        LoadWindowDropdown.AddOptions(options);
        LoadWindowDropdown.RefreshShownValue();
    }

    public void OnSaveButton()
    {
        var saveName = SaveWindowText.text;
        var saveManager = SaveManager.instance;
        saveManager.SaveGame();
        OnCancelButton();
    }
    
    public void OnCancelButton()
    {
        MainMenuButtons.SetActive(true);
        SaveWindow.SetActive(false);
        LoadWindow.SetActive(false);
    }
    
    public void OnLoadButton()
    {
        var saveName = LoadWindowDropdown.options[LoadWindowDropdown.value].text;
        var saveManager = SaveManager.instance;
        saveManager.LoadGame(saveName);
        OnCancelButton();
    }
}
