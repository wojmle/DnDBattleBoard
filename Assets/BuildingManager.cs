using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Assets.Scripts;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject[] objects;
    public GameObject pendingObject;
    [SerializeField] private Material[] materials;
    private Vector3 position;
    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;
    public Selection selection;
    public float rotateAmount;
    public float gridSize;
    bool isInsidePlane;
    bool gridOn = true;
    public int gridExtent = 10;         
    public Color gridColor = Color.green;
    public bool drawOnXZPlane = true;   
    public bool canPlace = true;
    [SerializeField] private Toggle gridToggle;
    private Material lineMaterial;
    private int objectIndex;
    public TMP_Dropdown charactersDropdown;
    private Material defaultMaterial;

    private string prefabPath = "Assets/Prefabs/Adversary";

    void Start()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabPath });
        prefabs = new GameObject[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            prefabs[i] = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        }

        // Ensure the TMP_Dropdown is assigned
        if (charactersDropdown != null)
        {
            var charactersList = new List<string>();
            foreach(var character in objects)
            {
                charactersList.Add(character.gameObject.name);
            }

            // Populate the TMP_Dropdown with these options
            PopulateDropdown(charactersList);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pendingObject != null)
        {
            if (selection.selectedObject != null)
            {
                selection.Deselect();
            }

            UpdateMaterials();
            if (gridOn)
            {
                //ToDo: Fix position and offset it on vertical direction
                pendingObject.transform.position = new Vector3(
                    RoundToNearestGrid(position.x),
                    position.y,
                    RoundToNearestGrid(position.z));
            }
            else
            {               
                //ToDo: Fix position and offset it on vertical direction
                pendingObject.transform.position = position;
            }

            if (Input.GetKeyUp(KeyCode.Escape) && pendingObject != null)
            {
                Destroy(pendingObject);
                pendingObject = null;
            }

            if (Input.GetMouseButtonDown(0) && canPlace && !isInsidePlane)
            {
                Destroy(pendingObject);
                pendingObject = null;
            }
            if (Input.GetMouseButtonDown(0) && canPlace && isInsidePlane)
            {
                PlaceObject();
            }
            if (Input.GetKeyDown(KeyCode.R) )
            {
                RotateObject();
            }
        }
    }

    public void PopulateDropdown(List<string> options)
    {
        // Clear existing options (optional)
        charactersDropdown.ClearOptions();

        // Create a new list to hold the TMP_Dropdown.OptionData objects
        List<TMP_Dropdown.OptionData> tmpOptions = new List<TMP_Dropdown.OptionData>();

        // Loop through the provided string options and create new OptionData objects
        foreach (string option in options)
        {
            TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData(option);
            tmpOptions.Add(newOption);
        }

        // Add the new options to the dropdown
        charactersDropdown.AddOptions(tmpOptions);
    }

    public void PlaceObject()
    {
        pendingObject.GetComponent<MeshRenderer>().material = defaultMaterial != null ? defaultMaterial : materials[2];
        pendingObject = null;
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            position = hit.point;
            isInsidePlane = true;
        }
        else
        {
            isInsidePlane = false;
        }
    }
    void OnRenderObject()
    {
        if (!gridOn) return; // Skip grid drawing if gridOn is false

        DrawGrid();
    }

    void OnEnable()
    {
        // Create a basic shader for the grid lines (if not assigned)
        lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
    }

    void DrawGrid()
    {
        // Apply the material
        lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);  // Start drawing lines
        GL.Color(gridColor); // Set the color for the lines

        // Draw the grid on the selected plane
        if (drawOnXZPlane)
        {
            DrawGridXZPlane();
        }

        GL.End();  // Finish drawing lines
    }

    private void DrawGridXZPlane()
    {
        for (int x = -gridExtent; x <= gridExtent; x++)
        {
            // Draw grid lines parallel to the Z axis
            GL.Vertex3(x * gridSize, 0f, -gridExtent * gridSize);
            GL.Vertex3(x * gridSize, 0f, gridExtent * gridSize);
        }

        for (int z = -gridExtent; z <= gridExtent; z++)
        {
            // Draw grid lines parallel to the X axis
            GL.Vertex3(-gridExtent * gridSize, 0f, z * gridSize);
            GL.Vertex3(gridExtent * gridSize, 0f, z * gridSize);
        }
    }

    void UpdateMaterials()
    {
        if (canPlace)
        {
            pendingObject.GetComponent<MeshRenderer>().material = materials[0];
        }
        else
        {
            pendingObject.GetComponent<MeshRenderer>().material = materials[1];
        }
    }

    public void InsertObject(Adversary adversary)
    {
        var selectedObjectFromDropdown = prefabs.FirstOrDefault(x => x.gameObject.name == adversary.Name);
        if (selectedObjectFromDropdown != null)
        {
            pendingObject = Instantiate(selectedObjectFromDropdown, position, transform.rotation);
            pendingObject.transform.rotation = Quaternion.Euler(90, 0, 0);
            pendingObject.tag = "Object";

            // Offset by half its height along its local Y direction
            var renderers = pendingObject.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
            {
                Bounds combinedBounds = renderers[0].bounds;
                for (int i = 1; i < renderers.Length; i++)
                    combinedBounds.Encapsulate(renderers[i].bounds);

                float halfHeight = combinedBounds.size.y / 2f;
                // Offset along the object's local Y (up) direction
                pendingObject.transform.position += Vector3.up * halfHeight;
            }

            // Get the EnemyController component from the instantiated object
            var enemyController = pendingObject.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.adversary = adversary;
            }
            defaultMaterial = pendingObject.GetComponent<MeshRenderer>().material;
        }
    }

    public void ToggleGrid()
    {
        if (gridToggle.isOn)
        {
            gridOn = true;
        }
        else
        {
            gridOn = false;
        }
    }

    float RoundToNearestGrid(float position)
    {
        float cellIndex = Mathf.Floor(position / gridSize);
        return (cellIndex * gridSize) + (gridSize / 2f);
    }

    public void RotateObject()
    {
        pendingObject.transform.Rotate(Vector3.up, rotateAmount);
    }
}
