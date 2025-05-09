using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllyObject", menuName = "Character/Create New Ally")]
public class AllyObject : ScriptableObject
{
    public string characterName;
    public string characterRace;
}
