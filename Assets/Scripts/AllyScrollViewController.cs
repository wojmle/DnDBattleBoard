using Assets;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllyScrollViewController : MonoBehaviour
{
    [SerializeField] private RectTransform contentPanel; // Assign in Inspector
    [SerializeField] private Button buttonPrefab; 
    
    private BuildingManager buildingManager;

    private void Start()
    {
        buildingManager = FindFirstObjectByType<BuildingManager>();
        EventManager.AddListener("AllyAdded", CreateAllyButton);
    }

    private void CreateAllyButton(object allyObject)
    {
        if (allyObject is Ally ally)
        {
            var buttonInstance = Instantiate(buttonPrefab, contentPanel);
            buttonInstance.GetComponentInChildren<TextMeshProUGUI>().text = ally.Name;
            buttonInstance.onClick.AddListener(() => OnAllyButtonClicked(ally));

            // Find the nested remove button by name
            var removeButton = buttonInstance.GetComponentsInChildren<Button>(true)
                .FirstOrDefault(b => b.gameObject.name == "RemoveAllyButton" && b != buttonInstance);

            if (removeButton != null)
            {
                removeButton.onClick.AddListener(() =>
                {
                    Destroy(buttonInstance.gameObject);
                });
            }
            else
            {
                Debug.LogWarning("AllyRemoveButton not found in button prefab.");
            }
        }
    }

    private void OnAllyButtonClicked(Ally ally)
    {
        // Find all AllyController components in the scene
        var allAllies = FindObjectsOfType<AllyController>();

        // Try to find an existing AllyController with the same Ally (by Name, Race, Model, or a unique property)
        var existing = System.Array.Find(allAllies, ac =>
            ac.allyObject != null &&
            ac.allyObject.Name == ally.Name &&
            ac.allyObject.Race == ally.Race &&
            ac.allyObject.Model == ally.Model);

        if (existing != null)
        {
            // Select the existing GameObject using the Selection script
            if (buildingManager.selection != null)
            {
                buildingManager.selection.Select(existing.gameObject);
            }
            else
            {
                Debug.LogWarning("Selection script not assigned in BuildingManager.");
            }
        }
        else
        {
            // No existing, insert new
            buildingManager.InsertObject(ally);
        }
    }

}
