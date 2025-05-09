using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public CharacterObject characterObject;
    private string characterRace;
    private string bieglosciBojowe;
    private string mroczneAtrybuty;
    public bool isEnemy;
    private Dictionary<string, int> characterStats = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        characterRace = characterObject.characterRace;
        bieglosciBojowe = characterObject.bieglosciBojowe;
        mroczneAtrybuty = characterObject.mroczneAtrybuty;
        isEnemy = characterObject.isEnemy;
        characterStats = new Dictionary<string, int>(characterObject.GetValues());
    }

    public void AddPoint(string key)
    {
        if (characterStats.ContainsKey(key))
        {
            characterStats[key] += 1;
        }
    }

    public void RemovePoint(string key)
    {
        if (characterStats.ContainsKey(key))
        {
            characterStats[key] -= 1;
        }
    }

    public string GetCharacterRace()
    {
        return characterRace; 
    }

    public string GetDarkAttributes()
    {
        return mroczneAtrybuty;
    }

    public string GetCombatPerks()
    {
        return bieglosciBojowe;
    }

    public Dictionary<string,int> GetCharacterStats()
    {
        return characterStats;
    }
}
