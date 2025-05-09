#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
[CustomEditor(typeof(CharacterObject))]
public class CharacterObjectEditor : Editor
{
    // Serialized properties to reference the fields from CharacterObject
    SerializedProperty characterRaceProp;
    SerializedProperty bieglosciBojoweProp;
    SerializedProperty mroczneAtrybutyProp;
    SerializedProperty isEnemyProp;
    SerializedProperty valuesProp;

    // Called when the inspector is loaded
    private void OnEnable()
    {
        // Find serialized properties for objectName, health, and the values list
        characterRaceProp = serializedObject.FindProperty("characterRace");
        bieglosciBojoweProp = serializedObject.FindProperty("bieglosciBojowe");
        mroczneAtrybutyProp = serializedObject.FindProperty("mroczneAtrybuty");
        isEnemyProp = serializedObject.FindProperty("isEnemy");
        valuesProp = serializedObject.FindProperty("values");
    }

    public override void OnInspectorGUI()
    {
        // Start updating the serialized object
        serializedObject.Update();

        // Reference to the CharacterObject scriptable object
        CharacterObject myScript = (CharacterObject)target;

        // Show the objectName and health properties in the inspector
        EditorGUILayout.PropertyField(characterRaceProp);
        EditorGUILayout.PropertyField(isEnemyProp);
        EditorGUILayout.PropertyField(bieglosciBojoweProp);
        EditorGUILayout.PropertyField(mroczneAtrybutyProp);

        // Show a label for predefined keys and values
        EditorGUILayout.LabelField("Predefined Keys and Values", EditorStyles.boldLabel);

        // Get predefined keys from the CharacterObject script
        List<string> keys = myScript.GetPredefinedKeys();

        // Loop through all keys and values, displaying them side-by-side
        for (int i = 0; i < keys.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();  // Start a new row for each key-value pair
            EditorGUILayout.LabelField(keys[i], GUILayout.MaxWidth(150));  // Display the key
            EditorGUILayout.PropertyField(valuesProp.GetArrayElementAtIndex(i), GUIContent.none);  // Display the value field
            EditorGUILayout.EndHorizontal();
        }

        // Apply modified properties back to the serialized object
        serializedObject.ApplyModifiedProperties();

        // Add a button to initialize the dictionary from the editor
        if (GUILayout.Button("Initialize Dictionary"))
        {
            myScript.InitializeDictionary();
        }
    }
}
#endif
