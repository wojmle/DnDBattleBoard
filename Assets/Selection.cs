using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Selection : MonoBehaviour
{
    public GameObject selectedObject;
    private BuildingManager buildingManager;
    public GameObject manipulateEnemyPanel;
    public GameObject allyStatsPanel;
    public GameObject enemyStatsPanel;
    private EnemyController enemyController;
    private AllyController ally;

    public TextMeshProUGUI attrLevel;
    public TextMeshProUGUI endurance;
    public TextMeshProUGUI might;
    public TextMeshProUGUI hate;
    public TextMeshProUGUI parry;
    public TextMeshProUGUI armour;
    public TextMeshProUGUI characterRace;
    public TextMeshProUGUI bieglosciBojowe;
    public TextMeshProUGUI mroczneAtrybuty;

    public TextMeshProUGUI allyName;
    public TextMeshProUGUI allyRace;

    private List<TextMeshProUGUI> textMeshList;

    // Start is called before the first frame update
    void Start()
    {
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
        textMeshList = new List<TextMeshProUGUI>
            {
                attrLevel,
                endurance,
                might,
                hate,
                parry,
                armour
            };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000)) 
            {
                if (hit.collider.gameObject.CompareTag("Object"))
                {
                    Select(hit.collider.gameObject);
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedObject != null)
        {
            Deselect();
        }
        if (Input.GetKeyDown(KeyCode.Delete) && selectedObject != null)
        {
            Delete();
        }
    }

    private void Select(GameObject obj)
    {
        if (obj == selectedObject) return;
        if (selectedObject != null) Deselect();
        Outline outline = obj.GetComponent<Outline>();
        if (outline == null) obj.AddComponent<Outline>();
        else outline.enabled = true;
        EnemyController characterController = obj.GetComponent<EnemyController>();
        AllyController allyController = obj.GetComponent<AllyController>();
        if (characterController != null && characterController.isEnemy)
        {
            enemyController = characterController;
            characterRace.text = characterController.GetCharacterRace();
            mroczneAtrybuty.text = characterController.GetDarkAttributes();
            bieglosciBojowe.text = characterController.GetCombatPerks();
            enemyStatsPanel.SetActive(true);

            foreach (var textMesh in textMeshList) 
            {        
                if (enemyController.GetCharacterStats().TryGetValue(textMesh.name, out var statValue))
                {
                    textMesh.text = statValue.ToString();
                }
            }
            manipulateEnemyPanel.SetActive(true);
        }
        if (allyController != null)
        {
            ally = allyController;
            allyRace.text = allyController.GetCharacterRace();
            allyName.text = allyController.GetCharacterName();
            allyStatsPanel.SetActive(true); 
            manipulateEnemyPanel.SetActive(true);
        }


        selectedObject = obj;
    }

    public void Deselect()
    {
        manipulateEnemyPanel.SetActive(false);
        allyStatsPanel.SetActive(false);
        enemyStatsPanel.SetActive(false);
        selectedObject.GetComponent<Outline>().enabled = false;
        selectedObject = null;
        enemyController = null;
    }

    public void Move()
    {
        buildingManager.pendingObject = selectedObject;
    }

    public void Delete()
    {
        GameObject objToDestroy = selectedObject;
        Deselect();
        Destroy(objToDestroy);
    }

    public void AddPoint(string key)
    {
        if (enemyController != null)
        {
            enemyController.AddPoint(key);
        }

        foreach (var textMesh in textMeshList)
        {
            if (enemyController.GetCharacterStats().TryGetValue(textMesh.name, out var statValue))
            {
                textMesh.text = statValue.ToString();
            }
        }
    }

    public void RemovePoint(string key)
    {
        if (enemyController != null)
        {
            enemyController.RemovePoint(key);
        }

        foreach (var textMesh in textMeshList)
        {
            if (enemyController.GetCharacterStats().TryGetValue(textMesh.name, out var statValue))
            {
                textMesh.text = statValue.ToString();
            }
        }
    }
}
