using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class JsonDataSave
{
    public List<int> decklist;

    public void Start()
    {
        decklist= new List<int>();
    }

    [ContextMenu("To Json Data")]
    void SaveDataToJson()
    {
        string jsonData = JsonUtility.ToJson(decklist);
        string path = Path.Combine(Application.dataPath, "PlayerDeck.JsonData");
        File.WriteAllText(path, jsonData);
    }
}
