using UnityEngine;

public interface IDataSave 
{
    void LoadData(GameData gameData);
    void SaveData(ref GameData gameData);
}
