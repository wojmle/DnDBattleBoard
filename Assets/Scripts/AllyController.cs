using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    public Ally allyObject;
    private NameBarUI nameBarUI;
    private string characterName;
    private string characterRace;

    // Start is called before the first frame update
    void Start()
    {
        characterRace = allyObject.Race;
        characterName = allyObject.Name;
    }

    public string GetCharacterRace()
    {
        return characterRace;
    }

    public string GetCharacterName()
    {
        return characterName;
    }

    public void SetUIBar(NameBarUI bar)
    {
        nameBarUI = bar;
    }

    public void RemoveUIBar()
    {
        if (nameBarUI != null)
        {
            Destroy(nameBarUI.gameObject);
            nameBarUI = null;
        }
    }
}
