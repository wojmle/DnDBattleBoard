using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName ="New Item", menuName = "Item/Create New Item")]
public class CharacterObject : ScriptableObject
{
    // Example properties in the ScriptableObject
    public string characterRace;
    public bool isEnemy;
    public string bieglosciBojowe;
    public string mroczneAtrybuty;

    // Predefined list of keys (traits or attributes for example)
    private readonly List<string> predefinedKeys = new List<string>
    {
        "ZAJAD£OŒÆ",
        "WYTRZYMA£OŒÆ",
        "POTÊGA",
        "NIENAWIŒÆ",
        "OBRONA",
        "PANCERZ"
    };

    // Values corresponding to each key
    [SerializeField]
    private List<int> values = new List<int> { 0, 0, 0, 0, 0, 0 };

    // Runtime dictionary for fast access (not serialized)
    private Dictionary<string, int> dictionary = new Dictionary<string, int>();

    // Initialize the dictionary using predefined keys and serialized values
    public void InitializeDictionary()
    {
        dictionary.Clear();
        for (int i = 0; i < predefinedKeys.Count; i++)
        {
            if (i < values.Count)
            {
                dictionary[predefinedKeys[i]] = values[i];
            }
        }
    }

    // Method to get values at runtime
    public int GetValue(string key)
    {
        if (dictionary.ContainsKey(key))
        {
            return dictionary[key];
        }

        Debug.LogError("Key not found: " + key);
        return -1; // Default value if key is not found
    }

    // Optionally expose a way to get all predefined keys
    public List<string> GetPredefinedKeys()
    {
        return predefinedKeys;
    }

    public Dictionary<string, int> GetValues()
    {
        InitializeDictionary();
        return dictionary;
    }
}
