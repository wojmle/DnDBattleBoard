using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    public AllyObject allyObject;
    private string characterName;
    private string characterRace;

    // Start is called before the first frame update
    void Start()
    {
        characterRace = allyObject.characterRace;
        characterName = allyObject.characterName;
    }

    public string GetCharacterRace()
    {
        return characterRace;
    }

    public string GetCharacterName()
    {
        return characterName;
    }
}
