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
    
    void Start()
    {
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
            PopulateDropdown();
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
                pendingObject.transform.position = new Vector3(
                    RoundToNearestGrid(position.x),
                    RoundToNearestGrid(position.y),
                    RoundToNearestGrid(position.z));
            }
            else
            {
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

    public void PopulateDropdown()
    {
        var adversaryConverter = new AdversaryToJsonClassConverter();
        var adversaries = adversaryConverter.CreateAdversaryList();
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

    public void InsertObject()
    {
        var selectedObjectFromDropdown = objects.Where(x=>x.gameObject.name == charactersDropdown.options[charactersDropdown.value].text).FirstOrDefault();
        pendingObject = Instantiate(selectedObjectFromDropdown, position, transform.rotation);
        defaultMaterial = pendingObject.GetComponent<MeshRenderer>().material;
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
        float xDiff = position % gridSize;
        position -= xDiff;
        if (xDiff > (gridSize/2))
        {
            position += gridSize;
        }
        return position;
    }

    public void RotateObject()
    {
        pendingObject.transform.Rotate(Vector3.up, rotateAmount);
    }
}
